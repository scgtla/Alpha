import axios from "axios";

export async function sendCredentials(login, password) {
    console.log({ login, password });

    const { data: { token } } = await axios.post('https://localhost:5001/api/Authorization', {
        login,
        password,
    });

    return token;
}