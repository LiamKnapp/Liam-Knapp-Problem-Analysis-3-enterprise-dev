import requests
import json

# Define the port where your backend API is running
PORT = 7280

headers = {
    'Content-Type': 'application/json'
}

# Note: your port number might be different
url = f'https://localhost:{PORT}/Quotes/add'

# Define the path to the text file containing quotes
file_path = 'Quotes.txt'

# Read quotes from the text file
with open(file_path, 'r') as file:
    quotes = file.read().split('\n.\n')  # Split quotes by '.\n'

    # Iterate over each quote and send a POST request to the API
for quote_text in quotes:
    # Split the quote into quote text and author
    parts = quote_text.split('--')
    if len(parts) == 2:
        quote = parts[0].strip()
        author = parts[1].strip()

        # Prepare the payload
        payload = {
            'quote': quote,
            'author': author
        }

        # use requests to POST this object as JSON:
        resp = requests.post(url, headers=headers, json=payload, verify=False)

        # If the resp was successful, the output the Location header:
        if resp.status_code == 201 and 'Location' in resp.headers:
            print("Quote:",quote)
            print("Author:",author)
            print(f'Posted quotes location: {resp.headers["Location"]}')
        else:
            print('Hmmm, there was a problem adding this new quote')
