$(document).ready(function () {
    const apiBaseUrl = '/Folder';

    function loadFolders() {
        $.get(apiBaseUrl, function (data) {
            $('#folderTable tbody').html(data);
        }).fail(function (jqXHR, textStatus, errorThrown) {
            console.error("Error loading folders:", textStatus, errorThrown);
            alert("An error occurred while loading folders. Please try again.");
        });
    }

    $(document).on('click', '.btn-delete', function (e) {
        e.preventDefault();
        const id = $(this).data('id');
        if (confirm('Are you sure you want to delete this folder?')) {
            $.ajax({
                url: `${apiBaseUrl}/Delete/${id}`,
                type: 'POST',
                data: { "__RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
                success: function () {
                    loadFolders();
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.error("Error deleting folder:", textStatus, errorThrown);
                    alert("An error occurred while deleting the folder. Please try again.");
                }
            });
        }
    });

    $('#createFolderForm').submit(function (e) {
        e.preventDefault();
        const formData = $(this).serialize();
        $.post(`${apiBaseUrl}/Create`, formData, function () {
            loadFolders();
            $('#createFolderModal').modal('hide');
        }).fail(function (jqXHR, textStatus, errorThrown) {
            console.error("Error creating folder:", textStatus, errorThrown);
            alert("An error occurred while creating the folder. Please try again.");
        });
    });

    // Initial load
    loadFolders();
});