# Create an ASP.NET MVC Ecommerce Site to Sell Laptops
# # Solution Named As E-Commerce Contains Three Projects-
1. DataLayer<br>
	1.1 AppDbContext – A class which inherits DbContext and DbSets to migrate in ssms.<br>
2. Models<br>
	2.1 ApplicationUser – A class which inherits IdentityClass and added property as address.<br>
	2.2 Register – A class containing properties like Username, EmailId, Password, Phone No, Address<br>
	2.3 Login – A class containing props like username,password while logging in the user.<br>
	2.4 LaptopModel – A class containing props for laptop like id, product name, price, imgurl, descritption.<br>
	2.5 OrderDetails – A class containing props like oid,pid, LaptopModel type of variable, Name, Address, Phone no.<br>
3. WebApp<br>
	3.1 Controllers<br>
		3.1.1 – AccountController – Which accepts requests for login or registering. <br>
			3.1.1.1 – RegisterAction handles to register the user and assigning roles like admin/normal user<br>
			3.1.1.2 – LoginAction handeles to login the user to provide actions like buying or see their order list.<br>
		3.1.2 – LaptopController – Which accepts request to display laptops<br>
		3.1.3 – OrderController – Use to handle order request of an product and decorated with authorize attribute.<br>
	3.2 View:<br>
		3.2.1 – Login,register,laptops



		
