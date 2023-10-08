import { useEffect, useState } from "react";
import { Container, Table, TableThead, TableTr, TableTh, TableTd, TableTbody } from "@mantine/core"
import axios from "axios";

interface Bill {
    id: string;
    name: string;
    category: string;
    createdDate: Date;
    dueDate: Date;
    paidDate?: Date;
    amountDue: number;
    amountPaid: number;
    isPaid?: boolean;
    isDeleted?: boolean;
}

export function Bills() {
    const [data, setData] = useState<Bill[]>([]);
    const url = 'https://localhost:7267/bills';

    useEffect(() => {
        axios.get(url)
            .then((response) => setData(response.data));
    }, []);

    return (
        <Container size="md">
            <Table withTableBorder>
                <TableThead>
                    <TableTr>
                        <TableTh>Nome</TableTh>
                        <TableTh>Categoria</TableTh>
                        <TableTh>Data de Vencimento</TableTh>
                        <TableTh>Data do Pagamento</TableTh>
                        <TableTh>Valor a Pagar</TableTh>
                        <TableTh>Valor Pago</TableTh>
                    </TableTr>
                </TableThead>
                <TableTbody>
                    {data.map(bill => {
                        return (<TableTr key={bill.id}>
                            <TableTd>{bill.name}</TableTd>
                            <TableTd>{bill.category}</TableTd>
                            <TableTd>{bill.dueDate.toString()}</TableTd>
                            <TableTd>{bill.paidDate?.toString()}</TableTd>
                            <TableTd>{bill.amountDue}</TableTd>
                            <TableTd>{bill.amountPaid}</TableTd>
                        </TableTr>)
                    })}
                </TableTbody>
            </Table>
        </Container>)
}