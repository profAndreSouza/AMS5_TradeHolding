"use client";

import { useState } from "react";
import { Button } from "@/components/ui/button";

interface DeleteUserDialogProps {
  userId: string;
  userName: string;
  onDelete: (id: string) => void;
  className: string;
}

export function DeleteUserDialog({ userId, userName, onDelete, className }: DeleteUserDialogProps) {
  const [open, setOpen] = useState(false);

  return (
    <>
      <Button variant="outline" className={className} onClick={() => setOpen(true)}>Excluir</Button>

      {open && (
        <div className="fixed inset-0 bg-black/50 z-40 flex items-center justify-center">
          <div className="bg-white p-6 rounded-xl shadow-xl space-y-4 max-w-sm w-full z-50">
            <h2 className="text-lg font-bold">Confirmar Exclusão</h2>
            <p>Tem certeza que deseja excluir <strong>{userName}</strong>?</p>
            <div className="flex justify-end gap-2">
              <Button variant="outline" onClick={() => setOpen(false)}>Cancelar</Button>
              <Button variant="danger" onClick={() => {
                onDelete(userId);
                setOpen(false);
              }}>
                Confirmar
              </Button>
            </div>
          </div>
        </div>
      )}
    </>
  );
}
