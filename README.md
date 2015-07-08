# louiemart
Flight run of nopcommerce site for MWT testing

Instructions to set up locally:

Prerequisites: Visual Studio 2013 Update 4, .Net 4.5.2

1. Clone the client decision service code: **git clone https://github.com/multiworldtesting/decision.git**

2. Clone the louiemart code to the same directory as (1) so your dir should contain: Decision, Louiemart: **git clone https://github.com/multiworldtesting/louiemart.git**

3. Switch to demo branch for both of these repos: **git checkout demo**

4. In Louiemart folder, run **localdbsetup.bat**. This sets up localdb instances for the website. 

5. Locate the **settings.txt** file in **louiemart\Presentation\Nop.Web\App_Data**. You can either delete this file or remove the values so that its content looks like below: 
      - DataProvider: 
      - DataConnectionString:

6. Build and run louiemart x64 configuration (other configs might work but not tested). If you get an error with loading incorrect format, it's likely because Visual Studio attempts to run IIS Express in 32-bit mode. To fix this, in VS 2013, go to Tools -> Options -> Projects and Solutions -> Web Projects -> Use the 64 bit version of IIS Express...

7. If things work you should get the Install page where you can specify:

   - Create Sample Data

   - Create any admin user & password

   - Use SQL Server data base [Recommended]

   - Create database if it doesn't exist

   - Sql Server name: (localdb)\LocalDBnopCommercev1

   - Database name: Louiemart

   - Use integrated Windows authentication

8. After install you will see the commerce website page with a sidebar containing some system trace messages

**Note**: The website is built for demo purposes only so will not work properly if there are multiple concurrent user sessions.
