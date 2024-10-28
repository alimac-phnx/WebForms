function addQuestion() {
    const container = document.getElementById('questions-container');
    const questionHtml = `
                <div class="question-item border rounded p-3 mb-3" draggable="true" ondragstart="drag(event)" id="question-${questionIndex}">
                    <h5>Question ${questionIndex + 1}</h5>
                    <div class="form-group mt-2">
                        <input type="text" class="form-control" name="Questions[${questionIndex}].Text" required />
                    </div>
                    <div class="form-group">
                        <label for="Questions[${questionIndex}].Type">Type</label>
                        <select name="Questions[${questionIndex}].Type" class="form-control">
                            <option value="SingleLine">Single-line answer</option>
                            <option value="MultipleLine">Text answer</option>
                            <option value="Integer">Integer-value answer</option>
                            <option value="Checkbox">Checkbox answer</option>
                        </select>
                    </div>
                    <div class="form-group form-check">
                        <input type="checkbox" class="form-check-input" name="Questions[${questionIndex}].IsVisible" id="Questions[${questionIndex}].IsVisible" value="true" checked />
                        <label class="form-check-label" for="Questions[${questionIndex}].IsVisible">Show in the form</label>
                    </div>

                    <button type="button" class="btn btn-danger mt-2" onclick="removeQuestion(${questionIndex})">Delete</button>
                </div>
            `;
    container.insertAdjacentHTML('beforeend', questionHtml);
    questionIndex++;

    updateDeleteButtonVisibility();
    updateQuestionOrder();
}

function removeQuestion(index) {
    const questionElement = document.getElementById(`question-${index}`);
    if (questionElement) {
        questionElement.remove();
    }

    updateDeleteButtonVisibility();
    updateQuestionOrder();
}

function updateDeleteButtonVisibility() {
    const deleteButtons = document.querySelectorAll('.question-item .btn-danger');
    if (deleteButtons.length === 1) {
        deleteButtons[0].style.display = 'none';
    } else {
        deleteButtons.forEach(button => button.style.display = 'block');
    }
}

function drag(ev) {
    ev.dataTransfer.setData("text", ev.target.id);
}

function updateQuestionOrder() {
    const questionsContainer = document.getElementById('questions-container');
    const questionItems = questionsContainer.querySelectorAll('.question-item');

    questionItems.forEach((item, index) => {
        item.id = `question-${index}`;

        const questionNumber = item.querySelector('h5');
        questionNumber.textContent = `Question ${index + 1}`;

        const textInput = item.querySelector('input[type="text"]');
        textInput.name = `Questions[${index}].Text`;

        const selectInput = item.querySelector('select');
        selectInput.name = `Questions[${index}].Type`;

        const isVisibleInput = item.querySelector('input[type="checkbox"]');
        isVisibleInput.name = `Questions[${index}].IsVisible`;

        const deleteButton = item.querySelector('button.btn-danger');
        deleteButton.setAttribute('onclick', removeQuestion(index));
    });
}

function drop(ev) {
    ev.preventDefault();
    const data = ev.dataTransfer.getData("text");
    const draggedElement = document.getElementById(data);
    const target = ev.target.closest('.question-item');

    if (target) {
        target.parentNode.insertBefore(draggedElement, target);
    } else {
        document.getElementById('questions-container').appendChild(draggedElement);
    }

    updateQuestionOrder();
}

function allowDrop(ev) {
    ev.preventDefault();
}

function showAutocomplete(value) {
    const tags = availableTags;
    const autocompleteList = document.getElementById('autocomplete-list');
    autocompleteList.innerHTML = '';

    if (!value) return false;

    let foundTag = false;

    tags.forEach(tag => {
        if (tag.toLowerCase().startsWith(value.toLowerCase()) && !selectedTags.includes(tag)) {
            const item = document.createElement('div');
            item.innerHTML = tag;
            item.onclick = () => {
                addTag(tag);
            };
            autocompleteList.appendChild(item);
            foundTag = true;
        }
    });

    if (!foundTag && value.trim().length > 0) {
        const addItem = document.createElement('div');
        addItem.innerHTML = `<button type="button" class="btn btn-primary btn-sm">Add tag "${value}"</button>`;
        addItem.onclick = () => {
            addTag(value);
        };
        autocompleteList.appendChild(addItem);
    }
}

function addTag(tag) {
    if (!selectedTags.includes(tag)) {
        selectedTags.push(tag);
        updateTagDisplay();
    }
}

function removeTag(tag) {
    selectedTags = selectedTags.filter(t => t !== tag);
    updateTagDisplay();
}

function updateTagDisplay() {
    const container = document.getElementById('selected-tags');
    container.innerHTML = '';

    selectedTags.forEach(tag => {
        const tagElement = document.createElement('div');
        tagElement.classList.add('tag-item', 'd-inline-block', 'mr-2', 'mb-2', 'p-2');
        tagElement.style.backgroundColor = '#e9ecef';

        const tagText = document.createElement('span');
        tagText.textContent = tag;

        const removeButton = document.createElement('button');
        removeButton.classList.add('btn', 'btn-sm', 'btn-danger', 'ml-2');
        removeButton.textContent = 'x';
        removeButton.onclick = () => {
            removeTag(tag);
        };

        tagElement.appendChild(tagText);
        tagElement.appendChild(removeButton);
        container.appendChild(tagElement);

        const hiddenInput = document.createElement('input');
        hiddenInput.type = 'hidden';
        hiddenInput.name = 'Tags';
        hiddenInput.value = tag;
        container.appendChild(hiddenInput);
    });
}

document.addEventListener('DOMContentLoaded', function () {
    updateTagDisplay();
});