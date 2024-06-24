# PostManagement

## Assignment Practice

##### Course: PRN221
##### Instructor: huongnt7
##### Type: Assignment 3 - SU24
##### Status: Not finish yet
##### Tech: SignalR, ASP.NET, EF, JavaScript...
##### Description: Long individual assignment at week 7 from huongnt7. Include CRUD, Search with multiple fields, filter, SignalR. Database generated using Code-First method.

-------------------------------------------------------------------------

# Main Functions
* Create database using Entity Framework Core.
* Member management, post management, and registration management: Read, Create, Update and Delete actions. 
* Search Post by ID , Title or Description
* Create a report statistics posts by the period from StartDate to EndDate, and sort post in descending order
* Member registration by Email and Password. 

# Database Design
![image](https://github.com/gnaohuv22/PostManagement/assets/101083424/bce9b570-1ea7-4a3d-a9db-e88f8fbb3bcd)

PostCategories (CategoryID, CategoryName, Description)
AppUsers (UserID, Fullname, Address, Email, Password)
Posts (PostID, AuthorID, CreatedDate, UpdatedDate, Title, Content, PublishStatus, CategoryID)