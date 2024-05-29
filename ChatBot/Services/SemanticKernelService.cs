using LLama.Common;
using LLama;
using LLamaSharp.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;


namespace ChatBot.Services
{
    public class SemanticKernelService
    {
        public ModelParams ModelParameters = null!;
        public LLamaWeights Model = null!;
        public LLamaContext Context = null;
        public InteractiveExecutor Executor = null!;

        public Kernel _kernel;
        public SemanticKernelService(KernelMemoryService memoryService)
        {
            var modelPath = @"D:\Faks\DiplomskiRadV2\ChatBot\ChatBot\LLModels\Meta-Llama-3-8B-Instruct-Q4_K_M.gguf";

            ModelParameters = new ModelParams(modelPath)
            {
                ContextSize = 4096,
                GpuLayerCount = 512
            };

            Model = LLamaWeights.LoadFromFile(ModelParameters);
            Context = new LLamaContext(Model, ModelParameters);
            Executor = new InteractiveExecutor(Context);


            var builder = Kernel.CreateBuilder();
            builder.Services.AddKeyedSingleton<IChatCompletionService>("local-bot", new LLamaSharpChatCompletion(Executor));
            builder.Services.AddLogging(c => c.SetMinimumLevel(LogLevel.Trace).AddConsole().AddDebug());

            _kernel = builder.Build();
        }
    }
}
