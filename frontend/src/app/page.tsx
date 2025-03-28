
export default function Home() {
  return (
    <div className="fixed top-0 left-0 w-full h-16 bg-primary 
                    shadow-md flex items-center px-6 justify-between">

      <div className="text-light text-lg font-bold">Trade Holding AMS</div>
      <div className="text-accent text-md font-medium">Página Inicial</div>
      <div className="flex items-center gap-4">
        <button className="p-2 bg-secondary text-light rounded-md">Tema</button>
        <button className="p-2 bg-secondary text-light rounded-md">Perfil</button>
      </div>
      
    </div>
  )
}
