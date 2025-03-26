
// Make sure the DOM is fully loaded then run this code
document.addEventListener('DOMContentLoaded', () => {
    const previewSize = 150;

    // Load darkmode on start from local storage
    var darkmode = localStorage.getItem('darkMode');
    if (darkmode === 'true') {
        document.body.classList.add('dark-mode');
        const dmbtn = document.querySelector('.darkmode-btn-toggle')
        if (dmbtn) {
            dmbtn.checked = true
        }
    }


    // Open Modal
    // Find all modals
    const modalButtons = document.querySelectorAll('[data-modal="true"]');
    modalButtons.forEach(button => {
        button.addEventListener('click', () => {
            const modalTarget = button.getAttribute('data-target');
            const modal = document.querySelector(modalTarget);

            // If the modal exists, show it
            if (modal) {
                modal.style.display = 'flex';
            }
        });
    });

    // Find all close buttons
    const modalCloseButtons = document.querySelectorAll('[data-close="true"]');
    modalCloseButtons.forEach(button => {
        button.addEventListener('click', () => {
            const modalTarget = button.closest('.modal');
            // If the modal exists, hide it
            if (modalTarget) {
                modalTarget.style.display = 'none';

                // Clear form input fileds
                modalTarget.querySelectorAll('form').forEach(form => {
                    form.reset();

                    const imagePreview = form.querySelector('.image-preview');
                    if (imagePreview) {
                        imagePreview.src = '';
                    }

                    const imagePreviewer = form.querySelector('.image-previewer');
                    if (imagePreviewer) {
                        imagePreviewer.classList.remove('selected');
                    }
                });
                
            }
        });
    });

    // Handle image preview
    document.querySelectorAll('.image-previewer').forEach(previewer => {
        const fileInput = previewer.querySelector('input[type="file"]');
        const imagePreview = previewer.querySelector('.image-preview');

        // Open file input when previewer is clicked
        previewer.addEventListener('click', () => {
            fileInput.click();
        });

        // Process image when file input changes
        fileInput.addEventListener('change', ({ target: { files } }) => {
            const file = files[0];
            if (file)
                processImage(file, imagePreview, previewer, previewSize);
        })
    });


    // Handle Submit forms
    const forms = document.querySelectorAll('.modal form');
    forms.forEach(form => {
        form.addEventListener('submit', async (e) => {
            e.preventDefault();

            clearErrorMessages(form);

            // Create a new form data object
            const formData = new FormData(form);
            try {
                const response = await fetch(form.action, {
                    method: 'post',
                    body: formData
                });

                // If Response is OK, close the modal and reload page
                if (response.ok) {
                    const modal = form.closest('.modal');
                    if(modal)
                        modal.style.display = 'none';

                    window.location.reload();
                }
                // If Response is not OK, show error messages
                else if (response.status === 400) {
                    const data = await response.json();

                    if (data.errors) {
                        Object.keys(data.errors).forEach(key => {
                            console.log(data)

                            // Find the input element that matches key value and add error class
                            const input = form.querySelector(`[name="${key}"]`)
                            if (input) {
                                input.classList.add('input-validation-error');
                            }
                            // Find the span element that matches key value and add error message
                            const errorSpan = form.querySelector(`[data-valmsg-for="${key}"]`);
                            if (errorSpan) {
                                errorSpan.innerText = data.errors[key].join('\n');
                                errorSpan.classList.add('field-validation-error');
                            }
                        });
                    }
                
                }
            }

            catch(error) {
                console.error("Failed to submit form: ", error);
            }

        });
    });
})



// Clear error messages
function clearErrorMessages(form) {
    // Clear all input validation messages
    form.querySelectorAll('[data-val="true"]').forEach(input => {
        input.classList.remove('input-validation-error');
    })

    // Clear all validation messages
    form.querySelectorAll('[data-valmsg-for]').forEach(span => {
        span.innerText = '';
        span.classList.remove('field-validation-error');
    })
}


// Load image
async function loadImage(file) {
    return new Promise((resolve, reject) => {
        // Create a new file reader
        const reader = new FileReader();

        // Create a new image
        reader.onerror = () => reject(new Error("Failed to load file"));
        reader.onload = (e) => {
            const img = new Image();
            img.onerror = () => reject(new Error("Failed to load image"));
            img.onload = () => resolve(img);
            img.src = e.target.result;
        }

        reader.readAsDataURL(file);
    });
}

// Process image
async function processImage(file, imagePreview, previewer, previewSize = 150) {
    try {
        // Load image
        const img = await loadImage(file);

        // Create canvas
        const canvas = document.createElement('canvas');

        // Set canvas size
        canvas.width = previewSize;
        canvas.height = previewSize;

        // Create 2d context
        const ctx = canvas.getContext('2d');

        // Draw image on canvas
        ctx.drawImage(img, 0, 0, previewSize, previewSize);

        // Update preview
        imagePreview.src = canvas.toDataURL('image/jpeg');
        previewer.classList.add('selected');


    } catch (error){
        console.error("Failed to process image: ", error);
    }
}