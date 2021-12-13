# Introduction
Astrow 2.0 remake of Astrow with new quality of life features, like
- Request flex that can be accepted or declinet (*Request can only happen every 20 mins E.g. 
10:00 - 10:20 - 10:40 - 11:00*) 
- Easier way to register absence & flex
- (*Ask instructor's if there is anything else they would like too see*)
# Shortcuts
*  **Change Log Updates**
	* [Change log](#Change-log)
		* [Unreleased](#Unreleased)
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
	* [Item's & Container's](#Item's-&-Container's)
		* [Container's](#Container's)
		* [Item's](#Item's)
	* [ToDO's](#ToDO's)

# Compatibility
### Browser Compatibility
- [x] FireFox
- [x] Google Chrome
- [x] Microsoft Edge

# Item's & Container's
### Container's
* **FileBox**
(Short description)
*  **InBox**
(Short description)
*  **LogedUser**
(Short description)
*  **TimeCard**
(Short description)
*  **Users**
(Short description)
### Item's
*  **Day's**
(Short description)
*  **File's**
(Short description)
*  **Message's**
(Short description)
*  **UserPersonalinfo** 
(Short description)
# ToDO's
- [ ] Admin page (*Create, Delete, Update User & Update User's Timecard*)
- [ ] Time kort (dynamiclly add month's from user start/end date) & add registration
- [ ] Side Fraværsanmodning
- [ ] Side Godkend period
- [ ] Side Terminalresultater
- [ ] Side Mine anmodninger
- [ ] Side Ferie kort
- [ ] Side Årlig oversigt
- [ ] Side Meddelelser
- [ ] Side Mine afvigelser
- [ ] Side Mine filer
- [ ] Side Indstillinger
- [ ] Side Ændre kodeord

# Change log
# [Unreleased]
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
