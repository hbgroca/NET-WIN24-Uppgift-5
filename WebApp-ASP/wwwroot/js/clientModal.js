// Client Modal Script, loads client data into the edit modal
// Creds to CoPilot
document.addEventListener('DOMContentLoaded', () => {
    const editButtons = document.querySelectorAll('.client-actions-edit-btn');

    editButtons.forEach(button => {
        button.addEventListener('click', async () => {
            const clientId = button.getAttribute('data-client-id');
            if (clientId) {
                try {
                    // Fetch client data
                    const response = await fetch(`/getclients/${clientId}`);
                    if (response.ok) {
                        const clientData = await response.json();

                        // Populate form fields
                        const form = document.querySelector('#editClientModal form');
                        form.querySelector('[name="Id"]').value = clientData.id;
                        form.querySelector('[name="Name"]').value = clientData.clientName;
                        form.querySelector('[name="Email"]').value = clientData.email;
                        form.querySelector('[name="Phone"]').value = clientData.phone;
                        form.querySelector('[name="Street"]').value = clientData.address.street;
                        form.querySelector('[name="ZipCode"]').value = clientData.address.zipCode;
                        form.querySelector('[name="City"]').value = clientData.address.city;
                        form.querySelector('[name="Country"]').value = clientData.address.country;
                        form.querySelector('[name="Status"]').value = clientData.status;

                        // Set image if available
                        const imagePreview = form.querySelector('.image-preview');
                        if (clientData.imageUrl) {
                            imagePreview.src = clientData.imageUrl;
                        } else {
                            imagePreview.src = '/images/User5.svg';
                        }
                    }
                } catch (error) {
                    console.error('Error fetching client data:', error);
                }
            }
        });
    });
});