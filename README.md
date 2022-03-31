# Introduction
Astrow 2.0 remake of Astrow with new quality of life features
- Request flex that can be accepted or declinet 
- Easier way to register absence & flex
- (*Ask instructor's if there is anything else they would like too see*)

# Shortcuts
*  **Change Log Updates**
	* [Change log](#Change-log)
		* [Unreleased](#Unreleased)
			* [Version 0.8.0](#Version-0.8.0)
				* [Added](#Version-0.8.0\Added)
				* [Updated](#Version-0.8.0\Updated)
			* [Version 0.7.0](#Version-0.7.0)
				* [Added](#Version-0.7.0\Added)
				* [Updated](#Version-0.7.0\Updated)
			*  [Version 0.6.0](#Version-0.6.0)
				* [Added](#Version-0.6.0\Added)
				* [Updated](#Version-0.6.0\Updated)
				* [Removed](#Version-0.6.0\Removed)
			*  [Version 0.5.0](#Version-0.5.0)
				* [Added](#Version-0.5.0\Added)
				* [Updated](#Version-0.5.0\Updated)
			* [Version 0.4.0](#Version-0.4.0)
				* [Added](#Version-0.4.0\Added)
				* [Updated](#Version-0.4.0\Updated)
			* [Version 0.3.0](#Version-0.3.0)
				* [Added](#Version-0.3.0\Added)
				* [Updated](#Version-0.3.0\Updated)
			* [Version 0.2.0](#Version-0.2.0)
				* [Added](#Version-0.2.0\Added)
				* [Updated](#Version-0.2.0\Updated)
			* [Version 0.1.0](#Version-0.1.0)
				* [Added](#Version-0.1.0\Added)
*  **Infomation**
	* [Compatibility](#Compatibility)
		* [Browser Compatibility](#Browser-Compatibility)
	* [Core Functions](#Core-Functions)
		* [Admins](#Admins)
		* [Users](#Users)
	* [ToDO's](#ToDO's)

# Compatibility
### Browser Compatibility
- [x] FireFox
- [x] Google Chrome
- [x] Microsoft Edge

# Core Functions
## Admins
### Create User's
* Page for creating user, users requires the following parameters.
 Firstame, Middlename, Lastname, Username, Password, Startdate and Endate (Middle name not required)
### Edit User
* Page for editing users parameter.
### Edit Time Card
* Page for changing a days, scan ind, scan out and abscense parameter
### Delete User
* Page for deleting users
###  Mark As Abscent
* Mark user as abscent
### Mark As Illegally Abscent
* Mark user as Illegally abscent
### Requests From User
* Page to accpet or deny request from user conserning abscens
## Users
### Clock In & Out
* Button on the nav bar for clocking in and out
### Request Abscens
* Button on the nav bar for requesting absense
### TimeCard 
* To kepp track of abscens & flex

# Version 2 ToDo's
- [ ] Site for scanning in with a card scanner
- [ ] Change how scanning works soo it's possible to scan in and out multipul time
- [ ] Change abscense time for the abscent reasons 
- [ ] Site Indstillinger (Change background idk)
- [ ] Site Ændre kodeord (Change password)

# Version 1 ToDO's
- [x] Admin page (*Create, Delete, Update User & Update User's Timecard*)
- [x] Time kort (dynamiclly add month's from user start/end date) & add registration
- [x] Site Fraværsanmodning
- [x] Site Godkend period (Accepted and Denyed, request)



# Change log
# Version 2
## Version 1.0.0 
### Added
# Version 1
## Version 0.8.0 ( Abscense Request / Small Fixes)
### Added
- [x]  Functionality to Request Abs page
- [x] Create sql table for requests
- [x] Create stored procedure to handle functionality of request page
- [x] SQL job that creates a day with that days date at 6 am every day
- [x] SQL Job that automatilcy marks users that hasn't scanned in as illeglay abscent at 9 am every day
### Updated
- [x] Check if username is available when making user
- [x] Create days between user start date and todays date
- [x] When accepting request update/create day
## Version 0.7.0 ( User Page / Abscens Request)
### Added
- [x]  Admin page for, Deleting/Mark as absent/Mark as illegally absent, users
- [x]  Functionality to User page 
- [x] Button functionality to request abscens e.g Flex, Sickness, Driving lessons...
- [x] Admin page to handle request
### Updated
- [x] Use of Opret Fraværes button
- [x] Styling of Opret Fraværes button
## Version 0.6.0 ( Admin Page Functionality)
### Added
- [x] Functionality to Create user page
- [x] Functionality to Update user page
- [x] Functionality to Update TimeCard page
### Updated
- [x] Styling for admin page's
- [x] Generally made more responsive
## Version 0.5.0 ( Clocking in & out)
### Added
- [x] On button press a small window opens, with the current time & a button for clocking in or out
- [x] Functionality behind clocking in & out for C# 
- [x] Functionality behind clocking in & out for SQL 
- [x] Functionality to live update current time disaply
### Updated
- [x] Register button use & styling
### Removed
- [x] Sorten buttons & dropdowns that wasn't needed
- [x] Classes that wasn't needed
- [x] Tables from SQL that wasn't needed
- [x] Procedures from SQL that wasn't needed

## Version 0.4.0 ( Time Card / Cleaning)
### Added
- [x] TimeCard functionality
- [x] TimeCard shows days from user's start date to end date
- [x] Ability to go one month back, forward and to todays date
- [x] Ability to search through days by month picker 
- [x] TimeCard styling
### Updated
- [x] Cleaned css file
- [x] New logo
- [x] Removed File, FileBox, Message, MessageInBox & TimeCard cs
## Version 0.3.0 (Admin page's / Time Card )
### Added
- [x] TimeCard CRUD
- [x] Date picker
- [x] JavaScript for Date picker
- [x] User CRUD
- [x] Admin page's CreateUser, DeleteUser, UpdateTimeCard & UpdateUser 
### Updated
- [x] Nav bar btn Admin page if logged user has "Instructor" role
## Version 0.2.0 (Home Page/Layout Page)
### Added
- [x] Layout page & Html/CSS/JavaScript for Layout
- [x] Home page & Html/CSS/JavaScript (*Time card is on home page*)
- [x] TimeCard Repository
### Updated
- [x] SQL Script more stored procedure's & change's to table's
- [x] Html/CSS for Login page
- [x] StoredProcedure.cs file updated with new procedure's
## Version 0.1.0 (Login & SQL)
### Added
- [x] SQL Database
- [x] SQL Script for creating database
- [x] Razor Pages Project Astrow 2.0
- [x] DataLayer folder with Stored Procedure
- [x] Container folder with Users, FileBox, InBox, LogedUser &TimeCard
- [x] Item folder with Days, Files, Messages & UserPersonalInfo
- [x] Repository for Users
- [x] _Login laytout page & Html/CSS for layout page
- [x] Login page & Html/CSS/JavaScript for Login page
- [x] Implement Login functionality
