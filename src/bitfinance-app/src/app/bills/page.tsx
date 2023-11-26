import { UserNav } from "../dashboard/components/user-nav";
import { Bill, columns } from "./columns"
import { DataTable } from "./data-table"

async function getData(): Promise<Bill[]> {
    // Fetch data from your API here.
    try {
        const res = await fetch('https://127.0.0.1:7267/bills', {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json'
            }
        });
        const data = await res.json();
        return data;
    } catch (err) {
        console.log(err);
        return [];
    }
}

export default async function BillsPage() {
    const data = await getData()
    console.log(data)

    return (
        <div className="hidden h-full flex-1 flex-col space-y-8 p-8 md:flex">
            <div className="flex items-center justify-between space-y-2">
                <div>
                    <h2 className="text-2xl font-bold tracking-tight">Hello there!</h2>
                    <p className="text-muted-foreground">
                        Here&apos;s a list of your bills for this month!
                    </p>
                </div>
                <div className="flex items-center space-x-2">
                    <UserNav />
                </div>
            </div>
            <DataTable data={data} columns={columns} />
        </div>
    )
}