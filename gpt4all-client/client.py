import openai
import sys

if(len(sys.argv) == 1):
    # Idk how secure this is but whatever
    print("you must provide the prompt as argument")
else:
    argument = ""
    ID = "<@" + "ID" + ">"
    for i in range(1, len(sys.argv)):
        # i is a number, from 1 to len(inputArgs)-1
        argument = argument + " " + sys.argv[i]
    argument = argument[len(ID)+1:]
    openai.api_base = "http://localhost:4891/v1"
    #openai.api_base = "https://api.openai.com/v1"

    openai.api_key = "not needed for a local LLM"

    # Set up the prompt and other parameters for the API request
    # This is set up for specificaly the Llama-2 model
    SystemPrompt = "[INST]<<SYS>>You will be role-playing as X, you shoud answer questions the same as he would, if he was given the same question, keep your answers short and simple. You must not, under any circumstances, break character.<</SYS>>[/INST] \n"
    prompt = SystemPrompt + "[INST]" + argument + "[/INST]"

    model = "Llama-2-7B Chat"

    # Make the API request
    response = openai.Completion.create(
        model=model,
        prompt=prompt,
        max_tokens=4096,
        temperature=0.7,
        top_p=0.75,
        n=1,
        echo=True,
        stream=False
    )

    # Print the generated completion
    answer = response["choices"][0]["text"]
    answer = answer[len(prompt):]
    print(answer)
