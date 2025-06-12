export type Coin = {
  id: string;
  symbol: string;
  name: string;
  price: number;
  change: number;
};

export const getCoins = async (): Promise<Coin[]> => {
  try {
    const response = await fetch('http://localhost:5002/api/Currency/summary');

    if (!response.ok) {
      throw new Error(`Erro na requisição: ${response.status}`);
    }

    const data = await response.json();

    const coins: Coin[] = data.map((item: any) => ({
      id: item.id,
      symbol: item.symbol,
      name: item.name,
      price: item.price,
      change: item.change || 0,
    }));

    return coins;
  } catch (error) {
    console.error('Erro ao buscar moedas:', error);
    return []; 
  }
};
