
document.addEventListener('DOMContentLoaded', () => {
    // Members popup part
    const searchBtn = document.querySelector('#editProjectModal .project-members-search-btn');
    const closeBtn = document.querySelector('#editProjectModal .close-members-btn');
    const membersWrapper = document.querySelector('#editProjectModal .project-members-add-wrapper');
    const membersList = document.querySelector('#editProjectModal .project-members-list');
    const availableMembersList = document.querySelector('#editProjectModal .project-members-add-list');

    let selectedMembers = [];

    // Open Members selection Popup
    searchBtn.addEventListener('click', function(e) {
        e.preventDefault();
        updateAvailableMembersList();
        membersWrapper.classList.add('add-popup-active');
    });

    // Close Members selection Popup
    closeBtn.addEventListener('click', function() {
        membersWrapper.classList.remove('add-popup-active');
    });

    // Add member on click
    availableMembersList.addEventListener('click', function(e) {
        if (e.target.classList.contains('add-member-btn'))
        {
            const listItem = e.target.closest('.project-members-add-list-object');
            const memberId = listItem.getAttribute('data-member-id');
            const memberImage = listItem.querySelector('.project-members-add-image').src;
            const memberName = listItem.querySelector('.project-members-add-name').textContent;

            // Add to selected members
            selectedMembers.push(memberId);

            // Create new member element in the selected list
            const newMember = document.createElement('div');
            newMember.className = 'project-members-list-member';
            newMember.setAttribute('data-member-id', memberId);
            newMember.innerHTML = `
                        <img class="project-members-list-member-image" src="${memberImage}"/>
                        <p class="project-members-list-member-name">${ memberName}</p>
                        <button type="button" class="remove-member-btn">✕</button>
                    `;

            membersList.appendChild(newMember);

            // Remove from available list
            listItem.style.display = 'none';

            // Update hidden input with selected members
            updateMembersInput();
        }
    });

    // Remove members from selected list
    membersList.addEventListener('click', function(e) {
        if (e.target.classList.contains('remove-member-btn'))
        {
            const listItem = e.target.closest('.project-members-list-member');
            const memberId = listItem.getAttribute('data-member-id');

            // Remove from selected members array
            selectedMembers = selectedMembers.filter(id => id !== memberId);

            // Remove from display
            listItem.remove();

            // Update available members list
            updateAvailableMembersList();

            // Update hidden input
            updateMembersInput();
        }
    });

    // Update id list (Hidden input)
    function updateMembersInput()
    {
        const chosenMembers = document.querySelectorAll('#editProjectModal .project-members-list-member');
        const selectedMemberIds = Array.from(chosenMembers).map(member => member.getAttribute('data-member-id'));
        document.querySelector('#editProjectModal .membersJsonInput').value = JSON.stringify(selectedMemberIds);
    }

    // Initialize selected members
    function initializeSelectedMembers() {
        selectedMembers = [];
        document.querySelectorAll('#editProjectModal .project-members-list-member').forEach(member => {
            const memberId = member.getAttribute('data-member-id');
            console.log(membersId)
            if (memberId) {
                selectedMembers.push(memberId);
            }
        });
    }

    // Initialize available members
    function updateAvailableMembersList() {
        const chosenMembers = document.querySelectorAll('#editProjectModal .project-members-list-member');
        const selectedMemberIds = Array.from(chosenMembers).map(member => member.getAttribute('data-member-id'));

        document.querySelectorAll('#editProjectModal .project-members-add-list-object').forEach(item => {
            const memberId = item.getAttribute('data-member-id');

            if (selectedMemberIds.includes(memberId)) {
                item.style.display = 'none';
            } else {
                item.style.display = 'flex';
            }
        });
    }


    // Initialize selected members
    initializeSelectedMembers();

    // Update available members list
    updateAvailableMembersList();

    // Update hidden input
    updateMembersInput();
});