import requests
import json

# Define the port where your backend API is running
PORT = 7280

# Define the headers for JSON content
headers = {
    'Content-Type': 'application/json'
}

# List to store selected tags
TagList = []

quote = input('Enter quote: ')
author = input('Enter author: ')

# Function to retrieve tags from the API
def GetTags():
    url = f'https://localhost:{PORT}/Tags'

    headers = {
        'Accept': 'application/json'
    }

    resp = requests.get(url, headers=headers, verify=False)

    if resp.status_code == 200:
        data = resp.json()
        print("Available Tags:")
        for tag in data:
            print(f"Tag Name: {tag['tagName']}   Tag ID: {tag['tagId']}")
        # Prompt the user to select tags
        while True:
            tagID = input("\nEnter the id of the associated tag name you want to add to the quote (0 to finish): ")
            if tagID == '0':
                break
            TagList.append(tagID)
    else:
        print(f"Failed to retrieve tags. Status code: {resp.status_code}")

# Function to create a new quote
def CreateQuote():
    # Define the URL to post the quote
    url = f'https://localhost:{PORT}/Quotes/add'

    # Prepare the payload
    payload = {
        'quote': quote,
        'author': author
    }

    # Send POST request to add the quote
    resp = requests.post(url, headers=headers, json=payload, verify=False)

    # If the response is successful and contains the Location header
    if resp.status_code == 201 and 'Location' in resp.headers:
        print("Quote:", quote)
        print("Author:", author)
        print(f'Posted quotes location: {resp.headers["Location"]}')
        quoteId = resp.headers["Location"].split('/')[-1]

        # Check if there are any tags to be added
        if TagList:
            for tagID in TagList:
                # Define the URL to add tag to quote
                url = f'https://localhost:{PORT}/Quotes/Tags/add/{quoteId}/{tagID}'
                # Send POST request to add tag to quote
                resp = requests.post(url, verify=False)
    else:
        print('There was a problem adding this new quote')

# Call the functions
GetTags()
CreateQuote()