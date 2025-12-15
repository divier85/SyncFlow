import loginClient from "./loginClient";

class LoginApi {

    async login(email: string, password: string) {

        const response = await loginClient.post(`/auth/login`, {
            email,
            password,
        });
        return response.data;
    }
}

export default new LoginApi();
