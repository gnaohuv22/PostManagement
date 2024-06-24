// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
ShowPosts();

var connection = new signalR.HubConnectionBuilder().withUrl("/CRUDHub").build();
connection.start().catch(err => console.error(err.toString()));

connection.on("LoadPost", function () {
    ShowPosts();
});

function ShowPosts() {
    let tbody = document.getElementById("postContainer");
    tbody.innerHTML = "";

    fetch("/Post/Index?handler=GetPosts")
        .then(res => res.json())
        .then(data => data.forEach(item => {
            console.log(item);
            let html = `<tr>
                            <td>${item.Title}</td>
                            <td>${item.Content}</td>
                            <td>${item.CategoryName}</td>
                            <td>${item.AuthorEmail}</td>
                            <td>${item.PublishStatus ? 'Published' : 'Not published'}</td>
                            <td>
                                <a href='/Post/Edit?id=${item.PostId}'>Edit</a> |
                                <a href='/Post/Detail?id=${item.PostId}'>Detail</a> |
                                <a href='/Post/Delete?id=${item.PostId}'>Delete</a>
                            </td>
                        </tr>`;
            tbody.innerHTML += html;
        }))
        .catch(error => console.error('Unable to load posts.', error));
}