interface HeaderProps {
    siteName?: string;
    pageName: string;
}

const Header: React.FC<HeaderProps> = ({siteName="Trade Holding AMS", pageName}) => {
    return(
        <header className="header-container bg-primary text-light flex justify-between items-center px-6 py-4">
            {/* Lado esquerdo: logo + nav */}
            <div className="header-left flex items-center gap-8">
                <div className="text-lg font-bold">{siteName}</div>

                <nav className="hidden lg:flex items-center gap-6">
                <a href="/compre" className="text-white hover:text-accent">Compre Cripto</a>
                <a href="/mercados" className="text-white hover:text-accent">Mercados</a>
                <div className="relative group">
                    <button className="text-white hover:text-accent flex items-center gap-1">
                    Trade
                    {/* <svg className="w-4 h-4 fill-current" viewBox="0 0 24 24">
                        <path d="M12.11 12.178L16 8.287l1.768 1.768-5.657 5.657-1.768-1.768-3.889-3.889 1.768-1.768 3.889 3.89z"/>
                    </svg> */}
                    </button>
                    {/* Submenu opcional aqui */}
                </div>
                {/* Adicione mais itens conforme precisar */}
                </nav>
            </div>

            {/* Lado direito: ações do usuário */}
            <div className="hidden md:block">
                <div className="flex items-center gap-4 ">
                    <button className="btn btn-primary">Entrar</button>
                    <button className="btn btn-accent">Cadastre‑se</button>
                </div>
            </div>  
        </header>
    )
}

export default Header;