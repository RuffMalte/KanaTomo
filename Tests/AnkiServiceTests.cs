
using Xunit;
using Xunit.Abstractions;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KanaTomo.Models.Anki;
using KanaTomo.Web.Services.Anki;
using KanaTomo.Web.Repositories.Anki;

namespace Tests
{
    public class AnkiServiceTests
    {
        private readonly Mock<IAnkiRepository> _ankiRepositoryMock;
        private readonly AnkiService _ankiService;
        private readonly ITestOutputHelper _output;

        public AnkiServiceTests(ITestOutputHelper output)
        {
            _ankiRepositoryMock = new Mock<IAnkiRepository>();
            _ankiService = new AnkiService(_ankiRepositoryMock.Object);
            _output = output;
        }

        [Fact]
        public async Task GetUserAnkiItemsAsync_ShouldReturnUserAnkiItems()
        {
            _output.WriteLine("Starting GetUserAnkiItemsAsync_ShouldReturnUserAnkiItems test");

            // Arrange
            var expectedItems = new List<AnkiModel>
            {
                new AnkiModel { Id = Guid.NewGuid(), Front = "Test1", Back = "Answer1" },
                new AnkiModel { Id = Guid.NewGuid(), Front = "Test2", Back = "Answer2" }
            };
            _ankiRepositoryMock.Setup(repo => repo.GetUserAnkiItemsAsync())
                .ReturnsAsync(expectedItems);

            _output.WriteLine($"Arranged test with {expectedItems.Count} expected items");

            // Act
            _output.WriteLine("Calling GetUserAnkiItemsAsync");
            var result = await _ankiService.GetUserAnkiItemsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedItems.Count, result.Count());
            _output.WriteLine($"Received {result.Count()} items");

            _output.WriteLine("Test completed successfully");
        }

        [Fact]
        public async Task GetAnkiItemByIdAsync_ShouldReturnCorrectItem()
        {
            _output.WriteLine("Starting GetAnkiItemByIdAsync_ShouldReturnCorrectItem test");

            // Arrange
            var expectedItem = new AnkiModel { Id = Guid.NewGuid(), Front = "Test", Back = "Answer" };
            _ankiRepositoryMock.Setup(repo => repo.GetAnkiItemByIdAsync(expectedItem.Id))
                .ReturnsAsync(expectedItem);

            _output.WriteLine($"Arranged test with expected item: Id={expectedItem.Id}");

            // Act
            _output.WriteLine("Calling GetAnkiItemByIdAsync");
            var result = await _ankiService.GetAnkiItemByIdAsync(expectedItem.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedItem.Id, result.Id);
            Assert.Equal(expectedItem.Front, result.Front);
            Assert.Equal(expectedItem.Back, result.Back);

            _output.WriteLine($"Received item: Id={result.Id}, Front={result.Front}, Back={result.Back}");
            _output.WriteLine("Test completed successfully");
        }

        [Fact]
        public async Task AddCardToUserAsync_ShouldReturnAddedCard()
        {
            _output.WriteLine("Starting AddCardToUserAsync_ShouldReturnAddedCard test");

            // Arrange
            var newCard = new AnkiModel { Front = "New Front", Back = "New Back" };
            var addedCard = new AnkiModel { Id = Guid.NewGuid(), Front = "New Front", Back = "New Back" };
            _ankiRepositoryMock.Setup(repo => repo.AddCardToUserAsync(newCard))
                .ReturnsAsync(addedCard);

            _output.WriteLine($"Arranged test with new card: Front={newCard.Front}, Back={newCard.Back}");

            // Act
            _output.WriteLine("Calling AddCardToUserAsync");
            var result = await _ankiService.AddCardToUserAsync(newCard);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(addedCard.Id, result.Id);
            Assert.Equal(addedCard.Front, result.Front);
            Assert.Equal(addedCard.Back, result.Back);

            _output.WriteLine($"Received added card: Id={result.Id}, Front={result.Front}, Back={result.Back}");
            _output.WriteLine("Test completed successfully");
        }

        [Fact]
        public async Task UpdateAnkiItemAsync_ShouldReturnUpdatedItem()
        {
            _output.WriteLine("Starting UpdateAnkiItemAsync_ShouldReturnUpdatedItem test");

            // Arrange
            var updatedItem = new AnkiModel { Id = Guid.NewGuid(), Front = "Updated Front", Back = "Updated Back" };
            _ankiRepositoryMock.Setup(repo => repo.UpdateAnkiItemAsync(updatedItem))
                .ReturnsAsync(updatedItem);

            _output.WriteLine($"Arranged test with updated item: Id={updatedItem.Id}, Front={updatedItem.Front}, Back={updatedItem.Back}");

            // Act
            _output.WriteLine("Calling UpdateAnkiItemAsync");
            var result = await _ankiService.UpdateAnkiItemAsync(updatedItem);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(updatedItem.Id, result.Id);
            Assert.Equal(updatedItem.Front, result.Front);
            Assert.Equal(updatedItem.Back, result.Back);

            _output.WriteLine($"Received updated item: Id={result.Id}, Front={result.Front}, Back={result.Back}");
            _output.WriteLine("Test completed successfully");
        }

        [Fact]
        public async Task DeleteAnkiItemAsync_ShouldReturnTrue()
        {
            _output.WriteLine("Starting DeleteAnkiItemAsync_ShouldReturnTrue test");

            // Arrange
            var itemId = Guid.NewGuid();
            _ankiRepositoryMock.Setup(repo => repo.DeleteAnkiItemAsync(itemId))
                .ReturnsAsync(true);

            _output.WriteLine($"Arranged test with item ID to delete: {itemId}");

            // Act
            _output.WriteLine("Calling DeleteAnkiItemAsync");
            var result = await _ankiService.DeleteAnkiItemAsync(itemId);

            // Assert
            Assert.True(result);

            _output.WriteLine($"Received result: {result}");
            _output.WriteLine("Test completed successfully");
        }

        [Fact]
        public async Task GetDueAnkiItemsAsync_ShouldReturnDueItems()
        {
            _output.WriteLine("Starting GetDueAnkiItemsAsync_ShouldReturnDueItems test");

            // Arrange
            var dueItems = new List<AnkiModel>
            {
                new AnkiModel { Id = Guid.NewGuid(), Front = "Due1", Back = "Answer1" },
                new AnkiModel { Id = Guid.NewGuid(), Front = "Due2", Back = "Answer2" }
            };
            _ankiRepositoryMock.Setup(repo => repo.GetDueAnkiItemsAsync())
                .ReturnsAsync(dueItems);

            _output.WriteLine($"Arranged test with {dueItems.Count} due items");

            // Act
            _output.WriteLine("Calling GetDueAnkiItemsAsync");
            var result = await _ankiService.GetDueAnkiItemsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(dueItems.Count, result.Count());

            _output.WriteLine($"Received {result.Count()} due items");
            _output.WriteLine("Test completed successfully");
        }

        [Fact]
        public async Task ReviewAnkiItemAsync_ShouldReturnReviewedItem()
        {
            _output.WriteLine("Starting ReviewAnkiItemAsync_ShouldReturnReviewedItem test");

            // Arrange
            var itemId = Guid.NewGuid();
            var difficulty = 3;
            var reviewedItem = new AnkiModel { Id = itemId, Front = "Reviewed", Back = "Answer", ReviewCount = 1 };
            _ankiRepositoryMock.Setup(repo => repo.ReviewAnkiItemAsync(itemId, difficulty))
                .ReturnsAsync(reviewedItem);

            _output.WriteLine($"Arranged test with item ID: {itemId}, difficulty: {difficulty}");

            // Act
            _output.WriteLine("Calling ReviewAnkiItemAsync");
            var result = await _ankiService.ReviewAnkiItemAsync(itemId, difficulty);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(itemId, result.Id);
            Assert.Equal(1, result.ReviewCount);

            _output.WriteLine($"Received reviewed item: Id={result.Id}, ReviewCount={result.ReviewCount}");
            _output.WriteLine("Test completed successfully");
        }

        [Fact]
        public async Task ResetAllCardsAsync_ShouldReturnTrue()
        {
            _output.WriteLine("Starting ResetAllCardsAsync_ShouldReturnTrue test");

            // Arrange
            _ankiRepositoryMock.Setup(repo => repo.ResetAllCardsAsync())
                .ReturnsAsync(true);

            // Act
            _output.WriteLine("Calling ResetAllCardsAsync");
            var result = await _ankiService.ResetAllCardsAsync();

            // Assert
            Assert.True(result);

            _output.WriteLine($"Received result: {result}");
            _output.WriteLine("Test completed successfully");
        }
    }
}