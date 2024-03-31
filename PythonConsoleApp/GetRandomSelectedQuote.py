import requests
import json
import random

# Define the port where your backend API is running
PORT = 7280

headers = {
        'Accept': 'application/json'
    }

# Function to get all quotes from your API
def get_all_quotes_Ids():
    url = f'https://localhost:{PORT}/Quotes/ids'

    # Send GET request to retrieve all quotes
    resp = requests.get(url, headers=headers, verify=False)

    # Check if the request was successful
    if resp.status_code == 200:
        # Print the JSON response
        print("Quote Ids Retrieved Successfully:")
        print(resp.json())

        # get a random number
        random_num = random.choice(resp.json())

        GetRandomQuote(random_num)
    else:
        print(f"Failed to retrieve quotes ids. Status code: {resp.status_code}")

def GetRandomQuote(random_num):
        url = f'https://localhost:{PORT}/Quotes/{random_num}'

        # Send GET request to retrieve all quotes
        resp = requests.get(url, headers=headers, verify=False)

        # Check if the request was successful
        if resp.status_code == 200:
            # Print the JSON response
            print("Random Quote Retrieved Successfully:")
            print(resp.json())
        else:
            print(f"Failed to retrieve random quotes. Status code: {resp.status_code}")

# Call the function to get all quotes and print the result
get_all_quotes_Ids()