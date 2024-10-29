
using System.Text;
using System.Text.Unicode;
using Azure;
using Azure.AI.OpenAI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

// Load the configuration
IConfiguration config = new ConfigurationBuilder()
                 .AddJsonFile("appsettings.json")
                 .Build();

// Get the configuration values
string oaiEndpoint = config["AzureOAIEndpoint"] ?? "";
string oaiKey = config["AzureOAIKey"] ?? "";
string oaiModelName = config["AzureOAIModelName"] ?? "";

// Create a client
OpenAIClient client =
new OpenAIClient(new Uri(oaiEndpoint), new AzureKeyCredential(oaiKey));

// Get the prompt from the user
Console.Write("Enter your prompt: ");
var UserPromptInput = Console.ReadLine();

// Set the options
var options = new ChatCompletionsOptions
{
    Messages = { new ChatRequestUserMessage(UserPromptInput) },
    DeploymentName = oaiModelName,
    MaxTokens = 150, Temperature = 0.5f
};

// Call the API
var response = client.GetChatCompletionsAsync(options).Result.Value;

// Print the response
Console.WriteLine("Response: " + response.Choices[0].Message.Content + "\n");

// Wait for the user to press a key
Console.WriteLine("Press any key to exit...");
Console.ReadKey();

///////////////////////////////////////////////////////////////////////////////////////
//加上 system prompt
options.Messages.Add(new ChatRequestSystemMessage("所有的回應都必須添加 '祝你今天愉快' 作為結尾。"));

// Get the prompt from the user
Console.Write("Enter your prompt: ");
UserPromptInput = Console.ReadLine();

// Call the API
response = client.GetChatCompletionsAsync(options).Result.Value;

// Print the response
Console.WriteLine("Response: " + response.Choices[0].Message.Content + "\n");

// Wait for the user to press a key
Console.WriteLine("Press any key to exit...");
Console.ReadKey();

///////////////////////////////////////////////////////////////////////////////////////
//加上 few-shot learning

// Get the prompt from the user
Console.Write("Enter your prompt: ");
UserPromptInput = Console.ReadLine();

options = new ChatCompletionsOptions
{
    Messages = {
        new ChatRequestSystemMessage("妳是判斷情緒的機器人，請根據以下對話判斷對話者的情緒。"),
        new ChatRequestUserMessage("我今天感覺很糟糕"),
        new ChatRequestAssistantMessage("負面情緒"),
        new ChatRequestUserMessage("我今天中樂透了"),
        new ChatRequestAssistantMessage("正面情緒"),
        new ChatRequestUserMessage(UserPromptInput) },
    DeploymentName = oaiModelName,
    MaxTokens = 150
};

// Call the API
response = client.GetChatCompletionsAsync(options).Result.Value;

// Print the response
Console.WriteLine("Response: " + response.Choices[0].Message.Content + "\n");

// Wait for the user to press a key
Console.WriteLine("Press any key to exit...");
Console.ReadKey();
