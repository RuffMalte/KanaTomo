
using Xunit;
using Moq;
using System.Threading.Tasks;
using KanaTomo.Web.Services.Translation;
using KanaTomo.Web.Repositories.Translation;
using KanaTomo.Models.Translation;
using Xunit.Abstractions;

namespace Tests
{
    public class TranslationServiceTests
    {
        private readonly Mock<ITranslationRepository> _mockRepository;
        private readonly TranslationService _translationService;
        private readonly ITestOutputHelper _output;

        public TranslationServiceTests(ITestOutputHelper output)
        {
            _mockRepository = new Mock<ITranslationRepository>();
            _translationService = new TranslationService(_mockRepository.Object);
            _output = output;
        }

        [Fact]
        public async Task Translate_ReturnsExpectedTranslation()
        {
            // Arrange
            var inputText = "Hello";
            var expectedTranslation = new TranslationModel(inputText)
            {
                JishoResponse = new JishoResponse
                {
                    Data = new List<JishoData>
                    {
                        new JishoData
                        {
                            Slug = "hello",
                            IsCommon = true,
                            Japanese = new List<JapaneseWord>
                            {
                                new JapaneseWord { Word = "こんにちは", Reading = "konnichiwa" }
                            },
                            Senses = new List<Sense>
                            {
                                new Sense { EnglishDefinitions = new List<string> { "hello", "good day" } }
                            }
                        }
                    }
                },
                DeeplResponse = new DeeplResponseModel
                {
                    Text = "こんにちは",
                    DetectedSourceLanguage = "EN",
                    BilledCharacters = 5
                }
            };

            _mockRepository.Setup(repo => repo.TranslateAsync(inputText))
                .ReturnsAsync(expectedTranslation);

            // Act
            _output.WriteLine($"Translating text: {inputText}");
            var result = await _translationService.Translate(inputText);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(inputText, result.OriginalText);
            Assert.NotNull(result.JishoResponse);
            Assert.NotNull(result.DeeplResponse);
            Assert.Equal("こんにちは", result.DeeplResponse.Text);
            Assert.Equal("EN", result.DeeplResponse.DetectedSourceLanguage);
            Assert.Equal(5, result.DeeplResponse.BilledCharacters);

            var jishoData = result.JishoResponse.Data.FirstOrDefault();
            Assert.NotNull(jishoData);
            Assert.Equal("hello", jishoData.Slug);
            Assert.True(jishoData.IsCommon);
            Assert.Equal("こんにちは", jishoData.Japanese.FirstOrDefault()?.Word);
            Assert.Equal("konnichiwa", jishoData.Japanese.FirstOrDefault()?.Reading);
            Assert.Contains("hello", jishoData.Senses.FirstOrDefault()?.EnglishDefinitions);

            _output.WriteLine("Translation test completed successfully");
        }
    }
}