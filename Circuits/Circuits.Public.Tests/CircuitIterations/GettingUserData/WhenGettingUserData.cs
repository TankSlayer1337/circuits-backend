using Amazon.DynamoDBv2.DocumentModel;
using Circuits.Public.DynamoDB;
using Circuits.Public.DynamoDB.Models.CircuitIteration;
using Circuits.Public.DynamoDB.Models.CircuitIteration.IterationModels;
using Circuits.Public.DynamoDB.Models.CircuitIteration.IterationModels.EquipmentInstance;
using Circuits.Public.DynamoDB.Models.CircuitIteration.IterationModels.ExerciseSet;
using Circuits.Public.PresentationModels.CircuitDefinitionModels;
using Circuits.Public.PresentationModels.CircuitRecordingModels;
using Circuits.Public.Tests.Utils;

namespace Circuits.Public.Tests.CircuitIterations.GettingUserData
{
    public class WhenGettingUserData : CircuitIterationRepositoryTestBase
    {
        [Fact]
        public async Task ForIterationsWithValidRequestAsync()
        {
            // GIVEN UserInfo endpoint is simulated
            var (userId, authorizationHeader) = UserInfoEndpointSimulator.SimulateUserInfoEndpoint(_httpClientWrapperMocker, _environmentVariableGetterMocker);

            // GIVEN a CircuitId
            var circuitId = _faker.Random.Guid().ToString();

            // GIVEN iterations exist
            var expectedCircuitIterationListings = CreateRandomCircuitIterationListings(circuitId, userId);

            // GIVEN corresponding circuit iteration entries
            var circuitIterationEntries = CreateIterationEntries(expectedCircuitIterationListings, userId);

            // GIVEN DynamoDB is simulated to return circuit iteration entries
            var circuitPointer = new CircuitPointer { UserId = userId, CircuitId = circuitId };
            _contextWrapperMocker.SimulateQueryAsync(circuitPointer, QueryOperator.BeginsWith, new string[] { string.Empty }, circuitIterationEntries);

            // WHEN retrieving iterations
            var circuitIterationRepository = BuildCircuitIterationRepository();
            var result = await circuitIterationRepository.GetIterationsAsync(authorizationHeader, circuitId);

            // THEN the correct iteration should be returned
            result.Should().BeEquivalentTo(expectedCircuitIterationListings);
        }

        [Fact]
        public async Task ForIterationWithValidRequestAsync()
        {
            // GIVEN UserInfo endpoint is simulated
            var (userId, authorizationHeader) = UserInfoEndpointSimulator.SimulateUserInfoEndpoint(_httpClientWrapperMocker, _environmentVariableGetterMocker);

            // GIVEN an iteration
            var iterationEntry = CreateRandomIterationEntry(userId);
            var circuitId = iterationEntry.CircuitPointer.CircuitId;
            var iterationId = iterationEntry.IterationId;

            // GIVEN DynamoDB is simulated to return CircuitIterationEntry
            _contextWrapperMocker.SimulateQueryAsync(iterationEntry.CircuitPointer, QueryOperator.Equal, new string[] { iterationId }, new List<CircuitIterationEntry>() { iterationEntry });

            // GIVEN expected circuit iteration data
            var expectedCircuitIteration = CreateRandomCircuitIteration(iterationEntry);

            // GIVEN TableQuerier is simulated to return iteration items from query
            SimulateTableQuerier(expectedCircuitIteration, iterationId, userId);

            // WHEN retrieving iteration
            var circuitIterationRepository = BuildCircuitIterationRepository();
            var result = await circuitIterationRepository.GetIterationAsync(authorizationHeader, circuitId, iterationId);

            // THEN the correct iteration should be returned
            result.Should().BeEquivalentTo(expectedCircuitIteration);
        }

        private void SimulateTableQuerier(CircuitIteration circuitIteration, string iterationId, string userId)
        {
            var circuitIterationPointer = new CircuitIterationPointer
            {
                UserId = userId,
                CircuitId = circuitIteration.CircuitId,
                IterationId = iterationId
            };
            var iterationQueryResult = CreateIterationQueryResult(circuitIteration, userId, iterationId);
            _tableQuerierMocker.SimulateRunIterationQueryAsync(circuitIterationPointer, iterationQueryResult);
        }

        private List<CircuitIterationListing> CreateRandomCircuitIterationListings(string circuitId, string userId)
        {
            var length = _faker.Random.Int(1, 5);
            var listings = new List<CircuitIterationListing>();
            for (var i = 0; i < length; i++)
            {
                var listing = new CircuitIterationListing
                {
                    CircuitId = circuitId,
                    IterationId = _faker.Random.Guid().ToString(),
                    DateStarted = _faker.Date.Past().ToString(),
                    DateCompleted = _faker.Random.Bool() ? string.Empty : _faker.Date.Recent().ToString()
                };
                listings.Add(listing);
            }
            return listings;
        }

        private List<CircuitIterationEntry> CreateIterationEntries(List<CircuitIterationListing> expectedCircuitIterationListings, string userId)
        {
            return expectedCircuitIterationListings.Select(listing => new CircuitIterationEntry
            {
                CircuitPointer = new CircuitPointer
                {
                    CircuitId = listing.CircuitId,
                    UserId = userId
                },
                IterationId = listing.IterationId,
                DateStarted = listing.DateStarted,
                DateCompleted = listing.DateCompleted
            }).ToList();
        }

        private CircuitIterationEntry CreateRandomIterationEntry(string userId, string? circuitId = null)
        {
            var circuitPointer = new CircuitPointer
            {
                CircuitId = circuitId ?? _faker.Random.Guid().ToString(),
                UserId = userId
            };
            var dateCompleted = _faker.Random.Bool() ? _faker.Date.Recent().ToString() : string.Empty;
            var entry = new CircuitIterationEntry
            {
                CircuitPointer = circuitPointer,
                IterationId = _faker.Random.Guid().ToString(),
                DateStarted = _faker.Date.Past().ToString(),
                DateCompleted = dateCompleted
            };
            return entry;
        }

        private IterationQueryResult CreateIterationQueryResult(CircuitIteration circuitIteration, string userId, string iterationId)
        {
            var recordedExerciseEntries = new List<RecordedExerciseEntry>();
            var exerciseSetEntries = new List<ExerciseSetEntry>();
            var equipmentInstanceEntries = new List<EquipmentInstanceEntry>();
            foreach (var exercise in circuitIteration.RecordedExercises)
            {
                var itemId = _faker.Random.Guid().ToString();
                var circuitIterationPointer = new CircuitIterationPointer
                {
                    CircuitId = circuitIteration.CircuitId,
                    UserId = userId,
                    IterationId = iterationId
                };
                var recordedExercisePointer = new RecordedExercisePointer
                {
                    ItemId = itemId,
                    OccurrenceId = _faker.Random.Guid().ToString()
                };
                var recordedExerciseEntry = new RecordedExerciseEntry
                {
                    CircuitIterationPointer = circuitIterationPointer,
                    RecordedExercisePointer = recordedExercisePointer,
                    ExerciseId = exercise.ExerciseId,
                    DateCompleted = exercise.DateCompleted
                };
                recordedExerciseEntries.Add(recordedExerciseEntry);

                foreach (var exerciseSet in exercise.ExerciseSets)
                {
                    var exerciseSetEntry = new ExerciseSetEntry
                    {
                        CircuitIterationPointer = circuitIterationPointer,
                        ExerciseSetPointer = new ExerciseSetPointer
                        {
                            ItemId = itemId,
                            OccurrenceId = recordedExercisePointer.OccurrenceId,
                            SetId = _faker.Random.Guid().ToString()
                        },
                        SetIndex = exerciseSet.Index,
                        RepetitionType = _faker.PickRandom<RepetitionType>(),
                        RepetitionMeasurement = exerciseSet.RepetitionSize
                    };
                    exerciseSetEntries.Add(exerciseSetEntry);

                    foreach (var equipmentItem in exerciseSet.EquipmentItems)
                    {
                        var equipmentInstanceEntry = new EquipmentInstanceEntry
                        {
                            CircuitIterationPointer = circuitIterationPointer,
                            EquipmentInstancePointer = new EquipmentInstancePointer
                            {
                                ItemId = itemId,
                                OccurrenceId = recordedExercisePointer.OccurrenceId,
                                SetId = exerciseSetEntry.ExerciseSetPointer.SetId,
                                EquipmentInstanceId = _faker.Random.Guid().ToString()
                            },
                            EquipmentId = equipmentItem.EquipmentId,
                            Count = equipmentItem.Count,
                            Load = equipmentItem.Load,
                            LoadUnit = (int)equipmentItem.LoadUnit
                        };
                        equipmentInstanceEntries.Add(equipmentInstanceEntry);
                    }
                }
            }

            return new IterationQueryResult
            {
                ExerciseEntries = recordedExerciseEntries,
                ExerciseSets = exerciseSetEntries,
                EquipmentInstances = equipmentInstanceEntries
            };
        }

        private CircuitIteration CreateRandomCircuitIteration(CircuitIterationEntry iterationEntry)
        {
            return new CircuitIteration
            {
                CircuitId = iterationEntry.CircuitPointer.CircuitId,
                RecordedExercises = CreateRandomRecordedExercises(),
                DateStarted = iterationEntry.DateStarted,
                DateCompleted = iterationEntry.DateCompleted
            };
        }

        private List<RecordedExercise> CreateRandomRecordedExercises()
        {
            var recordedExercises = new List<RecordedExercise>();
            var length = _faker.Random.Int(1, 6);
            for (var i = 0; i < length; i++)
            {
                var recordedExercise = new RecordedExercise
                {
                    ExerciseId = _faker.Random.Guid().ToString(),
                    DateCompleted = _faker.Date.Past().ToString(),
                    ExerciseSets = CreateRandomExerciseSets()
                };
                recordedExercises.Add(recordedExercise);
            }
            return recordedExercises;
        }

        private List<ExerciseSet> CreateRandomExerciseSets()
        {
            var exerciseSets = new List<ExerciseSet>();
            var length = _faker.Random.Int(1, 6);
            for (var i = 0; i < length; i++)
            {
                var exerciseSet = new ExerciseSet
                {
                    RepetitionSize = _faker.Random.Int(1, 100).ToString(),
                    EquipmentItems = CreateRandomEquipmentInstances()
                };
                exerciseSets.Add(exerciseSet);
            }
            return exerciseSets;
        }

        private List<EquipmentInstance> CreateRandomEquipmentInstances()
        {
            var instances = new List<EquipmentInstance>();
            var length = _faker.Random.Int(1, 6);
            for (var i = 0; i < length; i++)
            {
                var equipmentInstance = new EquipmentInstance
                {
                    EquipmentId = _faker.Random.Guid().ToString(),
                    Count = _faker.Random.Int(1, 2),
                    Load = _faker.Random.Float() * 100f,
                    LoadUnit = _faker.PickRandom<LoadUnit>()
                };
                instances.Add(equipmentInstance);
            }
            return instances;
        }
    }
}
