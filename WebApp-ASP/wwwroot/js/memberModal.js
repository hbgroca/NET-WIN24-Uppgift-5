// Member Modal Script, loads member data into the edit modal
// Creds to CoPilot for helping me with this
document.addEventListener('DOMContentLoaded', () => {
    const editButtons = document.querySelectorAll('.member-actions-edit-btn');

    editButtons.forEach(button => {
        button.addEventListener('click', async () => {
            const memberId = button.getAttribute('data-member-id');
            if (memberId) {
                try {
                    // Fetch member data
                    const response = await fetch(`/getmembers/${memberId}`);
                    if (response.ok) {
                        const memberdata = await response.json();

                        // Populate form fields
                        const form = document.querySelector('#editMemberModal form');
                        form.querySelector('[name="Id"]').value = memberdata.id;
                        form.querySelector('[name="FirstName"]').value = memberdata.firstName;
                        form.querySelector('[name="LastName"]').value = memberdata.lastName;
                        form.querySelector('[name="Email"]').value = memberdata.email;
                        form.querySelector('[name="Phone"]').value = memberdata.phone;
                        form.querySelector('[name="Street"]').value = memberdata.address.street;
                        form.querySelector('[name="ZipCode"]').value = memberdata.address.zipCode;
                        form.querySelector('[name="City"]').value = memberdata.address.city;
                        form.querySelector('[name="Country"]').value = memberdata.address.country;
                        form.querySelector('[name="Status"]').value = memberdata.status;
                        form.querySelector('[name="Title"]').value = memberdata.title;

                        // Extract year, month, and day from dateCreated
                        const birthDate = new Date(memberdata.birthDate);
                        form.querySelector('[name="Day"]').value = birthDate.getDate() ;
                        form.querySelector('[name="Month"]').value = birthDate.getMonth() + 1;
                        form.querySelector('[name="Year"]').value = birthDate.getFullYear() ;

                        // Set image if available
                        const imagePreview = form.querySelector('.image-preview');
                        if (memberdata.imageUrl) {
                            imagePreview.src = memberdata.imageUrl;
                        } else {
                            imagePreview.src = '/images/defaultprofile.png';
                        }
                    }
                } catch (error) {
                    console.error('Error fetching member data:', error);
                }
            }
        });
    });
});