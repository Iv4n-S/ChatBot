using Microsoft.KernelMemory;
using Microsoft.KernelMemory.SemanticKernel;
using LLamaSharp.KernelMemory;
using Microsoft.KernelMemory.AI;
using Microsoft.KernelMemory.Configuration;

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
                        MaxTokensPerParagraph = 256,
                        MaxTokensPerLine = 256,
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
    }
}
