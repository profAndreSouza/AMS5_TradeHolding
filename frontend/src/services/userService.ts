import axios from "axios";
import { userAPI } from "./api";

export interface User {
    id?: number;
    name: string;
    email: string;
    phone: string;
    address: string;
    password?: string;
    photo: string;
}

const userService = {

    async getAll(): Promise<User[]> {
        const header = {
            headers: {
                'Accept': 'application/json',
                'Access-Control-Allow-Origin': "*"
            }
        }
        const response = await axios.get(userAPI.getAll(), header);
        return response.data;
    }

}

export default userService;