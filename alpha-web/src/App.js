import './App.css';
import styled from 'styled-components';
import LoginForm from "./containers/LoginForm";

const Container = styled.div`
  
  height: 100vh;
  
  display: flex;
  flex-flow: column;
  
  justify-content: center;
  align-items: center;
`;

function App() {
  return (
    <Container className="App">
      <LoginForm/>
    </Container>
  );
}

export default App;
