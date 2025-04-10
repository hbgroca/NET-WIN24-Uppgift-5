// project Modal Script, loads project data into the edit modal
// Creds to CoPilot for helping me with this
document.addEventListener('DOMContentLoaded', () => {
    const editButtons = document.querySelectorAll('.project-actions-edit-btn');

    // Populate inputfields
    editButtons.forEach(button => {
        button.addEventListener('click', async () => {
            const projectId = button.getAttribute('data-project-id');
            if (projectId) {
                try {
                    // Fetch project data
                    const response = await fetch(`/getproject/${projectId}`);
                    if (response.ok) {
                        const memberdata = await response.json();

                        description = memberdata.client.description;

                        // Start initilize of rich text
                        initEditRichText('#project-edit-rich-text-editor', '#project-edit-rich-text-toolbar', '#editDescription', memberdata.description);

                        // Populate form fields
                        const form = document.querySelector('#editProjectModal form');
                        form.querySelector('[name="Id"]').value = memberdata.id;

                        form.querySelector('[name="ProjectName"]').value = memberdata.projectName;
                        form.querySelector('[name="ClientId"]').value = memberdata.client.id;
                        form.querySelector('[name="Description"]').value = memberdata.description;
                        form.querySelector('[name="StartDate"]').value = memberdata.startDate;
                        form.querySelector('[name="EndDate"]').value = memberdata.endDate;
                        form.querySelector('[name="Budget"]').value = memberdata.budget;
                        form.querySelector('[name="IsCompleted"]').value = memberdata.isCompleted;
                        const memberIds = memberdata.members.map(member => member.id);
                        form.querySelector('#membersJsonInput').value = JSON.stringify(memberIds);


                        const membersList = form.querySelector('#memberList');
                        membersList.innerHTML = '';

                        memberdata.members.forEach(member => {
                            const newMember = document.createElement('div');
                            newMember.className = 'project-members-list-member';
                            newMember.setAttribute('data-member-id', member.id);
                            newMember.innerHTML =
                                `
                                <img class="project-members-list-member-image" src="${member.imageUrl}" />
                                <p class="project-members-list-member-name">${member.fullName}</p>
                                <button type="button" class="remove-member-btn">✕</button>
                            `;

                            membersList.appendChild(newMember);
                        });

                        // Set image if available
                        const imagePreview = form.querySelector('.image-preview');
                        if (memberdata.imageUrl) {
                            imagePreview.src = memberdata.imageUrl;
                        } else {
                            imagePreview.src = '/images/defaultprofile.png';
                        }
                    }
                } catch (error) {
                    console.error('Error fetching project data:', error);
                }
            }
        });
    });
});

function initEditRichText(richTextEditorId, richTextToolbarId, textAreaId, content) {
    const textarea = document.querySelector(textAreaId);

    const quill = new Quill(richTextEditorId, {
        modules: {
            syntax: true,
            toolbar: richTextToolbarId
        },
        placeholder: 'Project description...',
        theme: 'snow'
    })

    if (content)
        quill.root.innerHTML = content;

    quill.on('text-change', () => {
        textarea.value = quill.root.innerHTML;
    })
}