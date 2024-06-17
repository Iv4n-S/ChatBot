using Microsoft.KernelMemory;
using LLamaSharp.KernelMemory;
using Microsoft.KernelMemory.Configuration;
using ChatBot.Models;
using System.Text.Json;
using System.Text.Encodings.Web;

namespace ChatBot.Services
{
    public class KernelMemoryService
    {
        public IKernelMemory _kernelMemory { get; set; }

        public KernelMemoryService(IConfiguration configuration)
        {
            _kernelMemory = new KernelMemoryBuilder()
                .WithLLamaSharpTextGeneration(new LLamaSharpConfig(@"D:\Faks\DiplomskiRadV2\ChatBot\ChatBot\LLModels\Meta-Llama-3-8B-Instruct-Q4_K_M.gguf"))
                .WithLLamaSharpTextEmbeddingGeneration(new LLamaSharpConfig(@"D:\Faks\DiplomskiRadV2\ChatBot\ChatBot\LLModels\nomic-embed-text-v1.5-Q5_K_M.gguf"))
                .WithCustomTextPartitioningOptions(
                    new TextPartitioningOptions
                    {
                        MaxTokensPerParagraph = 512,
                        MaxTokensPerLine = 512,
                        OverlappingTokens = 50
                    })
                .WithSimpleVectorDb(new Microsoft.KernelMemory.MemoryStorage.DevTools.SimpleVectorDbConfig()
                {
                    StorageType = Microsoft.KernelMemory.FileSystem.DevTools.FileSystemTypes.Disk
                }).Build();
        }


        public async Task<bool> StoreText(string text)
        {
            try
            {
                await _kernelMemory.ImportTextAsync(text);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> StoreFile(string path, string filename)
        {
            try
            {
                await _kernelMemory.ImportDocumentAsync(path, documentId: filename);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> StoreWebsite(string url)
        {
            try
            {
                await _kernelMemory.ImportWebPageAsync(url);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<string> AskQuestion(string question)
        {
            try
            {
                var answer = await _kernelMemory.SearchAsync(question, minRelevance: 0.75, limit: 2);

                List<MemoryResponseDto> response = new();
                if (answer?.Results == null)
                {
                    return "";
                }
                foreach (var answerItem in answer.Results) {
                    if (answerItem == null)
                        continue;
                    foreach (var partition in answerItem.Partitions)
                    {
                        response.Add(new MemoryResponseDto(partition.Text.Trim(), partition.Relevance));
                        SourcesList.AddSource(new SourceDto { FileId = answerItem.FileId, PartitionNumber = partition.PartitionNumber, Text = partition.Text.Trim(), Relevance = partition.Relevance });
                    }
                }
                return JsonSerializer.Serialize(response, new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping });
            }
            catch 
            {
                return "";
            }
        }
    }
}
