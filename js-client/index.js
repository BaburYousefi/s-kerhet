
document.addEventListener('DOMContentLoaded', () => {
  const todoBtnHtml = `<button id="listTodos">Visa alla ToDos</button><div id="allTodosList"></div>`;
  document.body.insertAdjacentHTML('beforeend', todoBtnHtml);
  document.getElementById('listTodos').addEventListener('click', listAllTodos);
});

async function listAllTodos() {
  const response = await fetch(`${baseApiUrl}/todos`, {
    method: 'GET',
    credentials: 'include',
  });
  const todosDiv = document.getElementById('allTodosList');
  if (response.ok) {
    const result = await response.json();
    const todos = result.data;
    todosDiv.innerHTML = '<h3>Alla ToDos:</h3>' + todos.map(t => `<div>${t.title ?? t.name ?? JSON.stringify(t)}</div>`).join('');
  } else {
    todosDiv.innerHTML = '<span style="color:red">Kunde inte hämta ToDos.</span>';
  }
}

document.addEventListener('DOMContentLoaded', () => {
  const userBtnHtml = `<button id="listUsers">Visa alla användare</button><div id="userList"></div>`;
  document.body.insertAdjacentHTML('beforeend', userBtnHtml);
  document.getElementById('listUsers').addEventListener('click', listAllUsers);
});

async function listAllUsers() {
  const response = await fetch(`${baseApiUrl}/accounts/ListAllUsers`, {
    method: 'GET',
    credentials: 'include',
  });
  const userListDiv = document.getElementById('userList');
  if (response.ok) {
    const users = await response.json();
    userListDiv.innerHTML = '<h3>Användare:</h3>' + users.map(u => `<div>${u.email} (${u.firstName} ${u.lastName})</div>`).join('');
  } else {
    userListDiv.innerHTML = '<span style="color:red">Kunde inte hämta användare.</span>';
  }
}
// Registreringsformulär
document.addEventListener('DOMContentLoaded', () => {
  const formHtml = `
    <h2>Registrera ny användare</h2>
    <form id="registerForm">
      <input type="email" id="regEmail" placeholder="E-post" required><br>
      <input type="text" id="regFirstName" placeholder="Förnamn" required><br>
      <input type="text" id="regLastName" placeholder="Efternamn" required><br>
      <input type="password" id="regPassword" placeholder="Lösenord" required><br>
      <button type="submit">Registrera</button>
    </form>
    <div id="registerResult"></div>
  `;
  document.body.insertAdjacentHTML('afterbegin', formHtml);

  document.getElementById('registerForm').addEventListener('submit', async (e) => {
    e.preventDefault();
    const Email = document.getElementById('regEmail').value;
    const FirstName = document.getElementById('regFirstName').value;
    const LastName = document.getElementById('regLastName').value;
    const Password = document.getElementById('regPassword').value;

    const response = await fetch(`${baseApiUrl}/accounts/registerUser`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ Email, FirstName, LastName, Password }),
    });

    const resultDiv = document.getElementById('registerResult');
    if (response.ok) {
      resultDiv.innerHTML = '<span style="color:green">Användare skapad!</span>';
      console.log('User registered successfully');
    } else {
      resultDiv.innerHTML = '<span style="color:red">Misslyckades att skapa användare.</span>';
    }
  });
});
const ToDoList = document.querySelector('#ToDoList');

document.querySelector('#displayToDo').addEventListener('click', listToDo);
document.querySelector('#login').addEventListener('click', login);
document.querySelector('#logout').addEventListener('click', logout);
document.querySelector('#register').addEventListener('click', register);

const baseApiUrl = 'https://localhost:5001/api';

async function listToDo() {
  console.log('List ToDo');

  const response = await fetch(`${baseApiUrl}/todos`, {
    method: 'GET',
    mode: 'cors',
    credentials: 'include',
  });

  if (response.ok) {
    const result = await response.json();
    console.log(result);
    displayProducts(result.data);
  } else {
    if (response.status === 401) displayError();
  }
}

async function login() {
  console.log('Log In');

  const response = await fetch(`${baseApiUrl}/accounts/login`, {
    method: 'POST',
    credentials: 'include',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({ Email: 'Erik@gmail.com', Password: 'Pa$$w0rd' }),
  });

  console.log(response);

  ToDoList.innerHTML = '';
}

async function register() {
  console.log('Register');

  const response = await fetch(`${baseApiUrl}/accounts/registerUser`, {
    method: 'POST',
    credentials: 'include',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({
      FirstName,
      LastName,
      Email,
      Password,
      RoleName
    }),
  });

  console.log(response);

  ToDoList.innerHTML = '';
}

async function logout() {
  console.log('Log out');

  const response = await fetch(`${baseApiUrl}/accounts/logout`, {
    method: 'POST',
    credentials: 'include',
  });

  console.log(response);
  ToDoList.innerHTML = '';
}

function displayProducts(todos) {
  ToDoList.innerHTML = '';

  for (let todo of todos) {
    const div = document.createElement('div');
    div.textContent = todo.name;

    ToDoList.appendChild(div);
  }
}

function displayError() {
  ToDoList.innerHTML = '<h2 style="color:red;">UNAUTHORIZED</h2>';
}

