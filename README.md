# louiemart
Flight run of nopcommerce site for MWT testing

Instructions to set up locally:

1. Clone the client decision service code: **git clone https://github.com/multiworldtesting/decision.git**

2. Clone the louiemart code to the same directory as (1) so your dir should contain: Decision, Louiemart: **git clone https://github.com/multiworldtesting/louiemart.git**

3. Switch to demo branch for both of these repos: **git checkout demo**

4. In Louiemart folder, run **localdbsetup.bat**. This sets up localdb instances for the website. 

5. Build and run louiemart x64 configuration (other configs might work but not tested).

6. If things work you should get the Install page where you can specify:

   - Create Sample Data

   - Create any admin user & password

   - Use SQL Server data base [Recommended]

   - Create database if it doesn't exist

   - Sql Server name: (localdb)\LocalDBnopCommercev1

   - Database name: Louiemart

   - Use integrated Windows authentication

7. After install you will see the commerce website page with a sidebar containing some system trace messages like below.
