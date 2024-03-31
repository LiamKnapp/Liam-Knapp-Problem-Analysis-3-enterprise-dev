// Define the port as a variable that can be easily changed
const PORT = 7280;

// Function to make AJAX request to get quotes data
function getQuotes() {
    fetch(`https://localhost:${PORT}/Quotes`)
        .then(response => response.json())
    .then(data => {
        // Clear existing table rows
        document.getElementById('quotes-body').innerHTML = '';

        // Loop through the received data and populate the table
        data.forEach(quote => {
            // Create a new row with a unique ID
            const row = document.createElement('tr');
            row.id = `quote-row-${quote.quoteId}`; // Assign a unique ID to each row

            row.innerHTML = `
                    <td>${quote.quote}</td>
                    <td>${quote.author}</td>
                    <td>${quote.likes}</td>
                    <td id="tags-${quote.quoteId}">Loading tags...</td>
                    <td>
                        <button onclick="editQuote(${quote.quoteId})">Edit</button>
                        <button onclick="likeQuote(${quote.quoteId})">Like</button>
                        <button onclick="deleteQuote(${quote.quoteId})">Delete</button>
                    </td>
                `;
            document.getElementById('quotes-body').appendChild(row);

            // Fetch tags for the current quote
            fetch(`https://localhost:${PORT}/Quotes/${quote.quoteId}/Tags`)
                .then(response => response.json())
                .then(tags => {
                    // Display tags for the current quote
                    const tagsElement = document.getElementById(`tags-${quote.quoteId}`);
                    tagsElement.textContent = tags.map(tag => tag.tagName).join(', ');
                })
                .catch(error => {
                    console.error(`Error fetching tags for quote ${quote.quoteId}:`, error);
                    // Display error message if tags couldn't be fetched
                    document.getElementById(`tags-${quote.quoteId}`).textContent = 'Error fetching tags';
                });
        });
    })
    .catch(error => console.error('Error fetching quotes:', error));
}

// Function to sort table rows by likes
function sortByLikes() {
    fetch(`https://localhost:${PORT}/Quotes/Likes`)
        .then(response => response.json())
        .then(data => {
            // Clear existing table rows
            document.getElementById('quotes-body').innerHTML = '';

            // Loop through the received data and populate the table
            data.forEach(quote => {
                // Create a new row with a unique ID
                const row = document.createElement('tr');
                row.id = `quote-row-${quote.quoteId}`; // Assign a unique ID to each row

                row.innerHTML = `
                    <td>${quote.quote}</td>
                    <td>${quote.author}</td>
                    <td>${quote.likes}</td>
                    <td id="tags-${quote.quoteId}">Loading tags...</td>
                    <td>
                        <button onclick="editQuote(${quote.quoteId})">Edit</button>
                        <button onclick="likeQuote(${quote.quoteId})">Like</button>
                        <button onclick="deleteQuote(${quote.quoteId})">Delete</button>
                    </td>
                `;
                document.getElementById('quotes-body').appendChild(row);

                // Fetch tags for the current quote
                fetch(`https://localhost:${PORT}/Quotes/${quote.quoteId}/Tags`)
                    .then(response => response.json())
                    .then(tags => {
                        // Display tags for the current quote
                        const tagsElement = document.getElementById(`tags-${quote.quoteId}`);
                        tagsElement.textContent = tags.map(tag => tag.tagName).join(', ');
                    })
                    .catch(error => {
                        console.error(`Error fetching tags for quote ${quote.quoteId}:`, error);
                        // Display error message if tags couldn't be fetched
                        document.getElementById(`tags-${quote.quoteId}`).textContent = 'Error fetching tags';
                    });
            });
        })
        .catch(error => console.error('Error fetching quotes:', error));
}

// Function to populate the dropdown menu with tags
function populateTagsDropdown() {
    fetch(`https://localhost:${PORT}/Tags`)
        .then(response => response.json())
        .then(tags => {
            const dropdown = document.getElementById('tags-dropdown');
            dropdown.innerHTML = ''; // Clear existing dropdown items

            // Add each tag as an item in the dropdown
            tags.forEach(tag => {
                const tagItem = document.createElement('a');
                tagItem.textContent = tag.tagName;
                tagItem.href = '#'; // Set a dummy href for styling (can be replaced with JavaScript event handling)
                tagItem.onclick = () => filterQuotesByTag(tag.tagId); // Pass tag ID when clicked
                dropdown.appendChild(tagItem);
            });
        })
        .catch(error => console.error('Error fetching tags:', error));
}

// Call the function to get quotes when the page loads
window.onload = function () {
    getQuotes();
    populateTagsDropdown();
    populateTagsAddQuoteDropdown();
};

// Function to filter quotes by tag
function filterQuotesByTag(tagId) {
    fetch(`https://localhost:${PORT}/Quotes/Tag/${tagId}`)
        .then(response => response.json())
        .then(quoteIds => {
            // Clear existing table rows
            document.getElementById('quotes-body').innerHTML = '';

            // Fetch details of each quote using its ID
            quoteIds.forEach(quoteId => {
                fetch(`https://localhost:${PORT}/Quotes/${quoteId}`)
                    .then(response => response.json())
                    .then(quote => {
                        // Populate the table with quote details
                        const row = document.createElement('tr');
                        row.innerHTML = `
                            <td>${quote.quote}</td>
                            <td>${quote.author}</td>
                            <td>${quote.likes}</td>
                            <td>Loading tags...</td>
                            <td>
                                <button onclick="editQuote(${quote.quoteId})">Edit</button>
                                <button onclick="likeQuote(${quote.quoteId})">Like</button>
                                <button onclick="deleteQuote(${quote.quoteId})">Delete</button>
                            </td>
                        `;
                        document.getElementById('quotes-body').appendChild(row);

                        // Fetch tags for the current quote
                        fetch(`https://localhost:${PORT}/Quotes/${quote.quoteId}/Tags`)
                            .then(response => response.json())
                            .then(tags => {
                                // Display tags for the current quote
                                const tagsElement = row.querySelector('td:nth-child(4)');
                                tagsElement.textContent = tags.map(tag => tag.tagName).join(', ');
                            })
                            .catch(error => {
                                console.error(`Error fetching tags for quote ${quote.quoteId}:`, error);
                                // Display error message if tags couldn't be fetched
                                row.querySelector('td:nth-child(4)').textContent = 'Error fetching tags';
                            });
                    })
                    .catch(error => {
                        console.error(`Error fetching quote ${quoteId}:`, error);
                    });
            });
        })
        .catch(error => console.error('Error fetching quotes by tag:', error));
}

// Function to show the add quote form
function showAddQuoteForm() {
    document.getElementById('add-quote-form').style.display = 'block';
}

// Function to hide the add quote form
function hideAddQuoteForm() {
    document.getElementById('add-quote-form').style.display = 'none';
}

// Function to populate the dropdown menu with tags for adding a new quote
function populateTagsAddQuoteDropdown() {
    fetch(`https://localhost:${PORT}/Tags`)
        .then(response => response.json())
        .then(tags => {
            const dropdown = document.getElementById('tags-dropdown-add-quote');
            dropdown.innerHTML = ''; // Clear existing dropdown items

            // Add each tag as an item in the dropdown
            tags.forEach(tag => {
                const tagItem = document.createElement('input');
                tagItem.type = 'checkbox';
                tagItem.value = tag.tagId;
                tagItem.id = `tag-${tag.tagId}`;
                const label = document.createElement('label');
                label.htmlFor = `tag-${tag.tagId}`;
                label.textContent = tag.tagName;
                dropdown.appendChild(tagItem);
                dropdown.appendChild(label);
                dropdown.appendChild(document.createElement('br'));
            });
        })
        .catch(error => console.error('Error fetching tags:', error));
}

// Function to add a new quote
function addNewQuote() {
    const quote = document.getElementById('quote-input').value;
    const author = document.getElementById('author-input').value;

    // Get selected tags
    const selectedTags = [];
    const tagsDropdown = document.getElementById('tags-dropdown-add-quote');
    const checkboxes = tagsDropdown.getElementsByTagName('input');
    for (let i = 0; i < checkboxes.length; i++) {
        if (checkboxes[i].type === 'checkbox' && checkboxes[i].checked) {
            selectedTags.push(checkboxes[i].value);
        }
    }

    // Make AJAX request to add new quote
    fetch(`https://localhost:${PORT}/Quotes/add`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            quote: quote,
            author: author
        })
    })
        .then(response => {
            if (!response.ok) {
                throw new Error('Failed to add new quote');
            }
            return response.json();
        })
        .then(data => {
            // Quote added successfully, now add tags
            const quoteId = data.quoteId;
            return Promise.all(selectedTags.map(tagId =>
                fetch(`https://localhost:${PORT}/Quotes/Tags/add/${quoteId}/${tagId}`, {
                    method: 'POST'
                })
            ));
        })
        .then(responses => {
            // Check if all tag additions were successful
            const allAdded = responses.every(response => response.ok);
            if (!allAdded) {
                throw new Error('Failed to add all tags to the quote');
            }
            // Tags added successfully, hide the form
            hideAddQuoteForm();
            getQuotes(); // Call the function to reload quotes
        })
        .catch(error => {
            console.error('Error adding new quote:', error);
        });
}

// Function to add a new Tag
function addNewTag() {
    const tagName = document.getElementById('TagName-input').value;

    // Make AJAX request to add new tag
    fetch(`https://localhost:${PORT}/Tags/add`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            tagName: tagName,
        })
    })
        .then(response => {
            if (!response.ok) {
                throw new Error('Failed to add new tag');
            }
            // Tags added successfully, hide the form
            hideAddTagForm();
            getQuotes();
            return response.json();
        })
        .catch(error => {
            console.error('Error adding new tag:', error);
        });
    getQuotes();
}

// Function to save changes after editing a quote
function saveChanges(quoteId) {
    const editedQuote = document.getElementById(`edited-quote-${quoteId}`).value;
    const editedAuthor = document.getElementById(`edited-author-${quoteId}`).value;

    // Make AJAX request to update the quote
    fetch(`https://localhost:${PORT}/Quotes/${quoteId}`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            quote: editedQuote,
            author: editedAuthor
        })
    })
        .then(response => {
            if (!response.ok) {
                throw new Error('Failed to update quote');
            }
            getQuotes(); // Reload quotes after updating
        })
        .catch(error => {
            console.error('Error updating quote:', error);
        });
}

// Function to cancel edits and revert back to original quote and author
function cancelEdits(quoteId) {
    getQuotes(); // Reload quotes to cancel edits
}

// Function to like a quote
function likeQuote(quoteId) {
    // Make AJAX request to add a like
    fetch(`https://localhost:${PORT}/Quotes/${quoteId}/Likes`, {
        method: 'PUT',
    })
        .then(response => {
            if (!response.ok) {
                throw new Error('Failed to increment likes');
            }
            else {
                getQuotes(); // Call the function to reload quotes
            }
            return response.json();
        })
        .catch(error => {
            console.error(`Error incrementing likes for quote ${quoteId}:`, error);
        });
}

// Function to delete a quote
function deleteQuote(quoteId) {
    // Implement delete functionality here
    console.log(`Deleting quote with ID ${quoteId}`);

    // Make AJAX request to delete a quote
    fetch(`https://localhost:${PORT}/Quotes/${quoteId}`, {
        method: 'DELETE',
    })
        .then(response => {
            if (!response.ok) {
                throw new Error('Failed to delete quote');
            }
            getQuotes(); // Call the function to reload quotes
            return response.json();

        })
        .catch(error => {
            console.error(`Error deleting quote ${quoteId}:`, error);
        });
}

// Function to show the add quote form
function showAddTagForm() {
    document.getElementById('add-Tag-form').style.display = 'block';

    populateTagsAddQuoteDropdown();
}

// Function to hide the add Tag form
function hideAddTagForm() {
    document.getElementById('add-Tag-form').style.display = 'none';
}

// Function to edit a quote
function editQuote(quoteId) {
    // Get the row of the quote to be edited
    const row = document.getElementById(`quote-row-${quoteId}`);

    // Find the quote and author elements in the row
    const quoteElement = row.querySelector('td:nth-child(1)');
    const authorElement = row.querySelector('td:nth-child(2)');

    // Get the current quote and author text
    const currentQuote = quoteElement.textContent;
    const currentAuthor = authorElement.textContent;

    // Create input fields to edit the quote and author
    const quoteInput = document.createElement('input');
    quoteInput.type = 'text';
    quoteInput.value = currentQuote;
    quoteElement.textContent = ''; // Clear existing content
    quoteElement.appendChild(quoteInput);

    const authorInput = document.createElement('input');
    authorInput.type = 'text';
    authorInput.value = currentAuthor;
    authorElement.textContent = ''; // Clear existing content
    authorElement.appendChild(authorInput);

    // Create a dropdown menu for selecting tags to add
    const dropdownAddTag = document.createElement('select');
    dropdownAddTag.id = `tags-dropdown-add-${quoteId}`;

    // Add a blank option to the dropdown
    const blankOptionAdd = document.createElement('option');
    blankOptionAdd

        .value = ''; // Set a blank value
    blankOptionAdd.textContent = 'Select a tag to add (optional)';
    dropdownAddTag.appendChild(blankOptionAdd);

    // Fetch tags not associated with the quote and add them as options in the dropdown
    fetch(`https://localhost:${PORT}/Tags`)
        .then(response => response.json())
        .then(tags => {
            // Add each tag as an option in the dropdown
            tags.forEach(tag => {
                const option = document.createElement('option');
                option.value = tag.tagId;
                option.textContent = tag.tagName;
                dropdownAddTag.appendChild(option);
            });
        })
        .catch(error => console.error('Error fetching tags:', error));

    // Create a dropdown menu for selecting tags to remove
    const dropdownRemoveTag = document.createElement('select');
    dropdownRemoveTag.id = `tags-dropdown-remove-${quoteId}`;

    // Add a blank option to the dropdown
    const blankOptionRemove = document.createElement('option');
    blankOptionRemove.value = ''; // Set a blank value
    blankOptionRemove.textContent = 'Select a tag to remove (optional)';
    dropdownRemoveTag.appendChild(blankOptionRemove);

    // Fetch tags associated with the quote and add them as options in the dropdown
    fetch(`https://localhost:${PORT}/Quotes/${quoteId}/Tags`)
        .then(response => response.json())
        .then(tags => {
            // Add each tag as an option in the dropdown
            tags.forEach(tag => {
                const option = document.createElement('option');
                option.value = tag.tagId;
                option.textContent = tag.tagName;
                dropdownRemoveTag.appendChild(option);
            });
        })
        .catch(error => console.error('Error fetching tags for quote:', error));

    // Add a button to save changes
    const saveButton = document.createElement('button');
    saveButton.textContent = 'Save Changes';
    saveButton.onclick = function () {
        // Update the quote with the new values
        const updatedQuote = quoteInput.value;
        const updatedAuthor = authorInput.value;

        // Make AJAX request to update the quote
        fetch(`https://localhost:${PORT}/Quotes/${quoteId}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                quote: updatedQuote,
                author: updatedAuthor,
            })
        })
            .then(response => {
                if (!response.ok) {
                    throw new Error('Failed to update quote');
                }
                getQuotes();

                // Check if a tag is selected for addition
                const selectedAddTagId = dropdownAddTag.value;
                if (selectedAddTagId) {
                    // If a tag is selected, add it to the quote
                    return fetch(`https://localhost:${PORT}/Quotes/Tags/add/${quoteId}/${selectedAddTagId}`, {
                        method: 'POST'
                    });
                }

                // Check if a tag is selected for removal
                const selectedRemoveTagId = dropdownRemoveTag.value;
                if (selectedRemoveTagId) {
                    // If a tag is selected, remove it from the quote
                    return fetch(`https://localhost:${PORT}/Quotes/${quoteId}/Tag/${selectedRemoveTagId}`, {
                        method: 'DELETE'
                    });
                }
            })
            .then(response => {
                if (!response || !response.ok) {
                    throw new Error('Failed to add tag to quote');
                }
                getQuotes();
            })
            .then(response => {
                if (!response || !response.ok) {
                    throw new Error('Failed to remove tag from quote');
                }
                // Call getQuotes() to refresh the quotes after updating
                getQuotes();
            })
            .catch(error => console.error('Error updating quote:', error));
    };

    // Add a button to cancel edits
    const cancelButton = document.createElement('button');
    cancelButton.textContent = 'Cancel Edits';
    cancelButton.onclick = function () {
        // Reload the quotes to cancel edits
        getQuotes();
    };

    // Add the dropdowns and buttons to the row
    row.appendChild(dropdownAddTag);
    row.appendChild(dropdownRemoveTag);
    row.appendChild(saveButton);
    row.appendChild(cancelButton);
}

// Function to edit a tag
function editTag(tagId) {
    // Implement edit tag functionality here
    console.log(`Editing tag with ID ${tagId}`);
}

// Function to delete a tag
function deleteTag(tagId) {
    // Implement delete tag functionality here
    console.log(`Deleting tag with ID ${tagId}`);
}

// Function to toggle dropdown menu
function toggleDropdown() {
    const dropdownContent = document.getElementById('tags-dropdown');
    dropdownContent.classList.toggle('show');
}

// Function to toggle dropdown menu
function toggleDropdown() {
    const dropdownContent = document.getElementById('tags-dropdown');
    dropdownContent.classList.toggle('show');
}