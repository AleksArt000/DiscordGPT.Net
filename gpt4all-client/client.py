import openai
import sys

if(len(sys.argv) == 1):
    print("you must provide the prompt as argument")
else:
    argument = ""
    ID = "<@" + "id" + ">"
    for i in range(1, len(sys.argv)):
        # i is a number, from 1 to len(inputArgs)-1
        argument = argument + " " + sys.argv[i]
    argument = argument[len(ID):]
    openai.api_base = "http://localhost:4891/v1"
    #openai.api_base = "https://api.openai.com/v1"

    openai.api_key = "not needed for a local LLM"

    # Set up the prompt and other parameters for the API request
    SystemPrompt = "###System: You are an ai assistant\n"
    promptTemplate = "You must respond in this format:\nUser: %1\nAssistant: \n###\nThe user said the following: \n"
    prompt = SystemPrompt + promptTemplate + argument

    model = "your-model"

    # Make the API request
    response = openai.Completion.create(
        model=model,
        prompt=prompt,
        max_tokens=4096,
        temperature=0.7,
        top_p=0.95,
        n=1,
        echo=True,
        stream=False
    )

    # Print the generated completion
    answer = response["choices"][0]["text"]
    answer = answer[len(prompt):]
    print(answer)