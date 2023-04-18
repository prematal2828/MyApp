var ptable;

$(document).ready(function () {
    ptable = $('#productTable').DataTable(
        {
            "ajax": {
                "url": "product/allproducts/"
            },
            "columns": [
                { "data": "name" },
                { "data": "description" },
                { "data": "price" },
                { "data": "category.name" },
                {
                    "data": "id",
                    "render": function (data) {
                        return `
                                <a href="/Admin/Product/CreateUpdate?id=${data}" > <i class="bi bi-pencil"></i> </a>
                                <a href="#" onClick=RemoveProduct("/Admin/Product/Delete/${data}")><i class="bi bi-trash"></i></a>
                                `
                    }
                }
            ]
        });
});

function RemoveProduct(url) {

    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            //Swal.fire(
            //    'Deleted!',
            //    'Your file has been deleted.',
            //    'success'
            //)

            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    if (data.success) {
                        ptable.ajax.reload();
                        toaster.success(data.message);
                    }
                    else {
                        toaster.error(data.message);
                    }
                }
            })


        }
    })

};
