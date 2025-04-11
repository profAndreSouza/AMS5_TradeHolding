"use client";

import { useState } from "react";
import { useRouter } from "next/navigation";
import { Button } from "@/components/ui/button";
import Header from "@/components/common/Header";

interface UserEditPageProps {
  params: { id: string };
}

export default function UserEditPage({ params }: UserEditPageProps) {
  const router = useRouter();
  const userId = params.id;

  const [form, setForm] = useState({
    name: "João Silva",
    email: "joao@email.com",
    role: "Admin",
    status: "Ativo",
  });

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    // Enviar dados para API aqui
    console.log("Atualizando usuário:", form);
    router.push(`/users/${userId}/view`);
  };

  return (
    <>
      <Header pageName="Usuários" />
      <div className="p-6 space-y-6 max-w-4xl mx-auto">
        <h1 className="text-2xl font-bold text-gray-800">Editar Usuário</h1>
        <form onSubmit={handleSubmit} className="space-y-4">
          <input
            type="text"
            placeholder="Nome"
            className="w-full border rounded px-4 py-2"
            value={form.name}
            onChange={(e) => setForm({ ...form, name: e.target.value })}
          />
          <input
            type="email"
            placeholder="Email"
            className="w-full border rounded px-4 py-2"
            value={form.email}
            onChange={(e) => setForm({ ...form, email: e.target.value })}
          />
          <select
            className="w-full border rounded px-4 py-2"
            value={form.role}
            onChange={(e) => setForm({ ...form, role: e.target.value })}
          >
            <option>Admin</option>
            <option>Técnico</option>
          </select>
          <select
            className="w-full border rounded px-4 py-2"
            value={form.status}
            onChange={(e) => setForm({ ...form, status: e.target.value })}
          >
            <option>Ativo</option>
            <option>Inativo</option>
          </select>

          <div className="flex justify-end space-x-2 pt-2">
            <Button
              type="button"
              variant="outline"
              onClick={() => router.push(`/users`)}
            >
              Cancelar
            </Button>
            <Button type="submit" variant="outline">
              Salvar Alterações
            </Button>
          </div>
        </form>
      </div>
    </>
  );
}
