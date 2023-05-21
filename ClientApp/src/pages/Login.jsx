import { useState } from "react";

function Login() {
    const [user, setUser] = useState("");
    const [password, setPassword] = useState("");
    const [response, setResponse] = useState("");

    const estilosDiv = {
        display: "flex",
        justifyContent: "center",
        alignItems: "center",
        height: "100vh",
        backgroundColor: "#031633",
    };

    const handleSubmit = async (event) => {
       event.preventDefault();
       const requestOptions = {
           method: "POST",
           headers: { "Content-Type": "application/json" },
           body: JSON.stringify({ usuario: user, contrasenia: password }),
       };
       const response = await fetch("http://localhost:5173/api/Account/login", requestOptions);
       const data = await response.json();
        setResponse(data);
        if (data.token != undefined) {
            localStorage.setItem('token', data.token);
            localStorage.setItem('user', data.nombreUsuario);
            localStorage.setItem('rol', data.rol);
            window.location.href = '/';
        }
    };    

    const handleUserChange = (event) => {
        const lowercaseValue = event.target.value.toLowerCase();
        const onlyLowerCase = lowercaseValue.replace(/[^a-z]/g, "");
        setUser(onlyLowerCase);
    };

    return (
        <>
            <div className="contenedor-login" style={estilosDiv}>
                <div className="card text-bg-dark mb-3">
                    <div className="card-body">
                        <h3>Bienvenido</h3>
                        <form onSubmit={handleSubmit}>
                            <div className="mb-3">
                                <label htmlFor="text" className="form-label">
                                    Usuario
                                </label>
                                <input
                                    type="text"
                                    className="form-control"
                                    id="inputUser"
                                    value={user}
                                    onChange={handleUserChange}
                                    required
                                />
                            </div>
                            <div className="mb-3">
                                <label htmlFor="inputPassword" className="form-label">
                                    Password
                                </label>
                                <input
                                    type="password"
                                    className="form-control"
                                    id="inputPassword"
                                    placeholder="Password"
                                    value={password}
                                    onChange={(event) => setPassword(event.target.value)}
                                    required
                                />
                            </div>
                            <button type="submit" className="btn btn-success">
                                Ingresar
                            </button>
                        </form>
                        {response && <p>{response.message}</p>}
                    </div>
                </div>
            </div>
        </>
    );
}

export default Login;
