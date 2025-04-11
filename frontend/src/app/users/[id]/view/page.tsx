"use client"

import Header from "@/components/common/Header";
import { Button } from "@/components/ui/button";
import { notFound } from "next/navigation";
import { useRouter } from "next/navigation";

interface UserViewPageProps {
  params: { id: string };
}

export default function UserViewPage({ params }: UserViewPageProps) {
  const router = useRouter();
  const userId = params.id;

  const user = {
    id: userId,
    name: "João Silva",
    email: "joao@email.com",
    role: "Admin",
    status: "Ativo",
  };

  if (!user) return notFound();

  return (
    <>
      <Header pageName="Usuários" />
      <div className="p-6 space-y-6 max-w-4xl mx-auto">
        <h1 className="text-2xl font-bold text-gray-800">Detalhes do Usuário</h1>

        <div className="bg-white border border-gray-200 rounded-lg shadow-sm p-6 space-y-4">
          <div>
            <p className="text-sm text-gray-500">Nome</p>
            <p className="text-lg font-medium text-gray-800">{user.name}</p>
          </div>

          <div>
            <p className="text-sm text-gray-500">Email</p>
            <p className="text-lg font-medium text-gray-800">{user.email}</p>
          </div>

          <div>
            <p className="text-sm text-gray-500">Papel</p>
            <p className="text-lg font-medium text-gray-800">{user.role}</p>
          </div>

          <div>
            <p className="text-sm text-gray-500">Status</p>
            <span
              className={`inline-block px-2 py-1 text-sm rounded-full font-medium ${
                user.status === "Ativo"
                  ? "bg-green-100 text-green-700"
                  : "bg-gray-100 text-gray-600"
              }`}
            >
              {user.status}
            </span>
          </div>
        </div>

        {/* Botões Voltar e Editar */}
        <div className="flex justify-end space-x-2 pt-2">
          <Button variant="outline" onClick={() => router.push("/users")}>
            Voltar
          </Button>
          <Button
            variant="outline"
            onClick={() => router.push(`/users/${userId}/edit`)}
          >
            Editar
          </Button>
        </div>
      </div>
    </>
  );
}
