import styled from "styled-components";
import {useRef, useState} from "react";
import {sendCredentials} from "../api";
import {saveToken} from "../utils/auth";

const Form = styled.div`
  border: 1px solid lightgray;
  border-radius: 3px;
  padding: 10px;
`;

const InputContainer = styled.div`
  margin: 5px;
`;

const LoginFailed = styled.div`
  color: red;
  display: ${(props) => props.visible ? 'block' : 'none'};
`;

function LoginForm() {
    const [loginFailed, setLoginFailed] = useState(false);

    const onClick = async () => {
        const username = loginInput.current.value;
        const password = passwordInput.current.value;

        console.log({
            username,
            password,
        });

        try {
            const token = await sendCredentials(username, password);

            saveToken(token);

            setLoginFailed(false);
        } catch (e) {
            setLoginFailed(true);
        }
    }

    const loginInput = useRef(null);
    const passwordInput = useRef(null);

    return (
        <Form>
            <LoginFailed visible={loginFailed}>Введены неверные данные</LoginFailed>
            <InputContainer>
                <label htmlFor='login'>Введите логин: </label>
                <br/>
                <input name='login' type='text' placeholder='Login' ref={loginInput} />
            </InputContainer>
            <InputContainer>
                <label htmlFor='password'>Введите пароль: </label>
                <br/>
                <input name='password' type='password' placeholder='Password' ref={passwordInput} />
            </InputContainer>
            <InputContainer>
                <input onClick={onClick} type='submit' value='Войти'/>
            </InputContainer>
        </Form>
    );
}

export default LoginForm;