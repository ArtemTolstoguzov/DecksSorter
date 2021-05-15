using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DecksSorter.Controllers;
using DecksSorter.DTO;
using DecksSorter.Models;
using DecksSorter.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace Tests
{
    public class DecksControllerTests
    {
        private const string ExistingDeckName = "b24d0ca6-c4c4-49f6-b6c2-8762fd6d7d72";
        private const string NotExistingDeckName = "11111111-2222-3333-4444-555555555555";
        
        private readonly List<string> testDecksNames;
        private readonly DeckDto testDeckDto;
        private readonly DeckConverter converter = new(new CardConverter());
        private readonly Mock<IDecksRepository> mockRepo = new();
        private readonly DecksController controller;
        
        public DecksControllerTests()
        {
            using var reader = new StreamReader("TestDecks.txt");
            var json = reader.ReadToEnd();
            var testDecks = JsonConvert.DeserializeObject<List<Deck>>(json).ToList();
            testDecksNames = testDecks.Select(deck => deck.Name).ToList();
            var testDeck = testDecks.First(deck => deck.Name == ExistingDeckName);
            testDeckDto = converter.Convert(testDeck);
            
            mockRepo.Setup(repo => repo.GetAllDecks())
                .Returns(testDecksNames);
            mockRepo.Setup(repo => repo.GetDeck(ExistingDeckName))
                .ReturnsAsync(testDeck);
            mockRepo.Setup(repo => repo.GetDeck(NotExistingDeckName))
                .ThrowsAsync(new ArgumentException());
            mockRepo.Setup(repo => repo.CreateDeck())
                .ReturnsAsync(testDeck);

            controller = new DecksController(mockRepo.Object, converter);
        }
        
        [Fact]
        public void GetAllDecksTest()
        {
            var result = controller.GetAllDecks();

            Assert.NotNull(result);
            var resultList = result.ToList();
            Assert.Equal(testDecksNames.Count, resultList.Count);
            Assert.Equal(testDecksNames.ToHashSet(), resultList.ToHashSet());
        }
        
        [Fact]
        public async Task GetExistingDeckTest()
        {
            var result = await controller.GetDeck(ExistingDeckName);

            Assert.NotNull(result);
            Assert.IsType<DeckDto>(result.Value);
            Assert.Equal(testDeckDto, result.Value);
        }
        
        [Fact]
        public async Task GetNotExistingDeckTest()
        {
            var result = await controller.GetDeck(NotExistingDeckName);

            Assert.NotNull(result);
            Assert.IsType<BadRequestResult>(result.Result);
        }
        
        [Fact]
        public async Task CreateDeckTest()
        {
            var result = await controller.CreateDeck();

            Assert.NotNull(result);
            var actionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.IsType<DeckDto>((DeckDto) actionResult.Value);
        }
        
        [Fact]
        public async Task DeleteExistingDeckTest()
        {
            var result = await controller.DeleteDeck(ExistingDeckName);

            Assert.NotNull(result);
            Assert.IsType<NoContentResult>(result);
        }
        
        [Fact]
        public async Task DeleteNotExistingDeckTest()
        {
            var result = await controller.DeleteDeck(NotExistingDeckName);

            Assert.NotNull(result);
            Assert.IsType<BadRequestResult>(result);
        }
        
        [Fact]
        public async Task ShuffleExistingDeckTest()
        {
            var result = await controller.ShuffleDeck(ExistingDeckName);

            Assert.NotNull(result);
            var actionResult = Assert.IsType<AcceptedAtActionResult>(result);
            Assert.IsType<DeckDto>((DeckDto) actionResult.Value);
        }
        
        [Fact]
        public async Task ShuffleNotExistingDeckTest()
        {
            var result = await controller.GetDeck(NotExistingDeckName);

            Assert.NotNull(result);
            Assert.IsType<BadRequestResult>(result.Result);
        }
    }
}