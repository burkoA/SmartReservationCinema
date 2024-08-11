<h1>Smart Reservation</h1>

<h3>Intro</h3>
<p>Hi! This is a simple project with integration of Google Maps and Google Calendar to simplify the process of booking or choosing a movie session.</p>

<h3>Installation steps</h3>
<ul>
  <li>Press the green button at the top of this page <code>Code</code> 📂.</li>
  <li>Copy link.</li>
  <li>Open terminal window and write <strong>git clone [paste the link]</strong>.</li>
  <li>Open file <strong>SmartReservationCinema.sln</strong> by using Visual Studio.</li>
  <li>Change database connection string in folder <strong>FilmContext</strong> in class <strong>FilmDbContext.cs</strong> in 33 line</li>
  <li>Change API for Google Maps in folder <strong>Controller</strong> in class <strong>FilmController</strong> in method <strong>SortByDistance</strong>.</li>
  <li>Change credentials for Google Maps in class <strong> Startup.cs</strong> in line 72 and 73: <strong>ClientId</strong> and <strong>ClientSecret</strong>.</li>
  <li>After creating database in <strong>Package Manager Console</strong> write <strong>update-database</strong> for migration and change database to correct view.</li> 
</ul>

<h3>Example of working</h3>
<h4>Example of sorting cinema from closest to farthest</h4>
<p>This is screen with user profile and example adress for test.</p>
<img src="https://github.com/user-attachments/assets/db896478-c90e-4515-8b23-097fd1e0e3c1">
<p>This is screen with sorting film from closest to farthest.</p>
<img src="https://github.com/user-attachments/assets/758c2b96-d7ee-401d-a2e3-25d109766202">
<h4>Example of creating an event</h4>

<p>Screen with the form and permission that the user must give.</p>
<img width="50%" src="https://github.com/user-attachments/assets/7f85b0b8-87fd-498d-8c1a-470abc9d4537">
<img width="50%" src="https://github.com/user-attachments/assets/ba7fe523-ef4b-4236-9132-c02a46f41dc3">

<p>Screen with creating event in calendar</p>
<img width="70%" src="https://github.com/user-attachments/assets/6745c356-e1e1-43a4-a7cc-008490d15866">

<h3>Feedback & Contacts</h3>
<p>You can send me mail: arsenburko67@gmail.com or find me in telegram <strong>@arssssssssssssssssssss</strong></p>
