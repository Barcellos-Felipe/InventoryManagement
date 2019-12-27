This database file exists only for testing purposes.
You should delete the 'database.db' file, create another and run sqlScript.sql from terminal using the commands:
	* sqlite3 database.db
	* .read sqlScript.sql

- There are two tables in the database:
	* Product
	* ProductTest

 ** Structure for both tables 
 
	 INTEGER   idProduct
	 TEXT	   code
	 TEXT	   description
	 INTEGER   quantity
	 REAL      price

 ** After any changes in the database structure, verify the class 'DatabaseHandler.cs'
