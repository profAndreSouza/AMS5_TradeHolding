"use client";

import { useRouter } from "next/navigation";
import Header from "@/components/common/Header";
import { Button } from "@/components/ui/button";
import { DeleteUserDialog } from "@/components/dialogs/DeleteUserDialog";
import { useState } from "react";

export default function UserListPage() {
  const router = useRouter();

  const [users, setUsers] = useState([
    {
      id: "1",
      name: "João Silva",
      email: "joao@email.com",
      role: "Administrador",
      status: "Ativo",
    },
    {
      id: "2",
      name: "Maria Souza",
      email: "maria@email.com",
      role: "Técnico",
      status: "Inativo",
    },
  ]);

  const handleDelete = (id: string) => {
    setUsers((prev) => prev.filter((user) => user.id !== id));
  };

  return (
    <>
      <Header pageName="Usuários" />
      <div className="p-6 space-y-6">
        <div className="flex items-center justify-between">
          <h1 className="text-2xl font-bold">Usuários</h1>
          <Button
            className="bg-blue-600 hover:bg-blue-700 text-white"
            onClick={() => router.push("/users/new")}
          >
            Adicionar Usuário
          </Button>
        </div>

        <div className="flex justify-end">
          <input
            type="text"
            placeholder="Buscar usuário..."
            className="border border-gray-300 rounded-lg px-4 py-2 w-full max-w-sm"
          />
        </div>

        <div className="overflow-x-auto">
          <table className="min-w-full text-sm bg-white border rounded-lg">
            <thead className="bg-gray-100">
              <tr>
                <th className="text-left px-4 py-2">Nome</th>
                <th className="text-left px-4 py-2">Email</th>
                <th className="text-left px-4 py-2">Papel</th>
                <th className="text-left px-4 py-2">Status</th>
                <th className="text-left px-4 py-2">Ações</th>
              </tr>
            </thead>
            <tbody>
              {users.map((user) => (
                <tr key={user.id} className="border-t">
                  <td className="px-4 py-2">{user.name}</td>
                  <td className="px-4 py-2">{user.email}</td>
                  <td className="px-4 py-2">{user.role}</td>
                  <td className="px-4 py-2">
                    <span
                      className={
                        user.status === "Ativo"
                          ? "text-green-600 font-medium"
                          : "text-gray-500"
                      }
                    >
                      {user.status}
                    </span>
                  </td>
                  <td className="px-4 py-2 space-x-2">
                    <Button
                        variant="outline"
                        className="border-blue-600 text-blue-600 hover:bg-transparent"
                        onClick={() => router.push(`/users/${user.id}/view`)}
                    >
                        Visualizar
                    </Button>
                    <Button
                        variant="outline"
                        className="border-yellow-500 text-yellow-500 hover:bg-transparent"
                        onClick={() => router.push(`/users/${user.id}/edit`)}
                    >
                        Editar
                    </Button>
                    <DeleteUserDialog
                      userId={user.id}
                      userName={user.name}
                      onDelete={handleDelete}
                      className="border-red-600 text-red-600 hover:bg-transparent"
                    />
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>

        {/* Paginação */}
        <div className="flex justify-end pt-4">
          <nav className="space-x-2">
            <button className="px-3 py-1 bg-gray-200 rounded hover:bg-gray-300">
              Anterior
            </button>
            <button className="px-3 py-1 bg-gray-200 rounded hover:bg-gray-300">
              Próximo
            </button>
          </nav>
        </div>
      </div>
    </>
  );
}
