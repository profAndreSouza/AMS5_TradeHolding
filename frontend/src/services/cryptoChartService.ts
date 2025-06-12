export type ChartPoint = {
  time: string;
  value: number;
};

export const getChartData = async (id: string, quantity: number): Promise<ChartPoint[]> => {
  try {
    const response = await fetch(`http://localhost:5002/api/Currency/${id}/chart?quantity=${quantity}`);
    if (!response.ok) {
      throw new Error('Erro ao buscar dados do gráfico');
    }

    const data = await response.json();
    console.log(data);

    const chartData: ChartPoint[] = data.map((item: any) => ({
      time: new Date(item.time).toLocaleTimeString('pt-BR', {
        hour: '2-digit',
        minute: '2-digit',
      }),
      value: item.value,
    }));

    return chartData;
  } catch (error) {
    console.error('Erro ao carregar dados do gráfico:', error);
    return [];
  }
};
