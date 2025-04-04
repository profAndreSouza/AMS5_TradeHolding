"use client"
import { useEffect, useState } from "react";
import userService, {User} from "@/services/userService";

export default function UsersPage() {
    const [users, setUsers] = useState<User[]>([]);
    const [error, setError] = useState<String>("");

    useEffect(() => {
        async function fetchUsers() {
            try {
                const data = await userService.getAll();
                setUsers(data);
            } catch (err) {
                setError(`Erro ao buscar usuários: ${err}`);
            }
        }
        fetchUsers();
    }, []);

    return (
        <div>
            <h1>Lista de Usuários</h1>
            <p>{error}</p>
            <ul>
                {users.map((user) => (
                    <li key={user.id}>
                        {user.name}
                    </li>
                ))}
            </ul>
        </div>
    )
}