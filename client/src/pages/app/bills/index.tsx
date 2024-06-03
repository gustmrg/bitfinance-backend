import { Badge } from "@/components/ui/badge";
import { Button } from "@/components/ui/button";
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table";

export function Bills() {
    const bills = []

    return (
        <>
            <div className="mx-auto grid w-full gap-2">
                <h1 className="text-3xl font-semibold md:text-2xl">Bills</h1>
            </div>
            {bills.length === 0 ? (
                <div
                className="flex flex-1 items-center justify-center rounded-lg border border-dashed shadow-sm" x-chunk="dashboard-02-chunk-1"
                    >
                    <div className="flex flex-col items-center gap-1 text-center">
                        <h3 className="text-2xl font-bold tracking-tight">
                            You have no bills
                        </h3>
                        <p className="text-sm text-muted-foreground">
                            You can start tracking your expenses as soon as you add a bill.
                        </p>
                        <Button className="mt-4">Add Bill</Button>
                    </div>
                </div>
            ) : (
                <div
                className="flex flex-1 items-center justify-center rounded-lg border-solid shadow-sm"
                    >                    
                    <Table>
                        <TableHeader>
                        <TableRow>
                            <TableHead>Customer</TableHead>
                            <TableHead className="hidden sm:table-cell">
                            Type
                            </TableHead>
                            <TableHead className="hidden sm:table-cell">
                            Status
                            </TableHead>
                            <TableHead className="hidden md:table-cell">
                            Date
                            </TableHead>
                            <TableHead className="text-right">Amount</TableHead>
                        </TableRow>
                        </TableHeader>
                        <TableBody>
                        <TableRow className="bg-accent">
                            <TableCell>
                            <div className="font-medium">Liam Johnson</div>
                            <div className="hidden text-sm text-muted-foreground md:inline">
                                liam@example.com
                            </div>
                            </TableCell>
                            <TableCell className="hidden sm:table-cell">
                            Sale
                            </TableCell>
                            <TableCell className="hidden sm:table-cell">
                            <Badge className="text-xs" variant="secondary">
                                Fulfilled
                            </Badge>
                            </TableCell>
                            <TableCell className="hidden md:table-cell">
                            2023-06-23
                            </TableCell>
                            <TableCell className="text-right">$250.00</TableCell>
                        </TableRow>
                        <TableRow>
                            <TableCell>
                            <div className="font-medium">Olivia Smith</div>
                            <div className="hidden text-sm text-muted-foreground md:inline">
                                olivia@example.com
                            </div>
                            </TableCell>
                            <TableCell className="hidden sm:table-cell">
                            Refund
                            </TableCell>
                            <TableCell className="hidden sm:table-cell">
                            <Badge className="text-xs" variant="outline">
                                Declined
                            </Badge>
                            </TableCell>
                            <TableCell className="hidden md:table-cell">
                            2023-06-24
                            </TableCell>
                            <TableCell className="text-right">$150.00</TableCell>
                        </TableRow>
                        <TableRow>
                            <TableCell>
                            <div className="font-medium">Noah Williams</div>
                            <div className="hidden text-sm text-muted-foreground md:inline">
                                noah@example.com
                            </div>
                            </TableCell>
                            <TableCell className="hidden sm:table-cell">
                            Subscription
                            </TableCell>
                            <TableCell className="hidden sm:table-cell">
                            <Badge className="text-xs" variant="secondary">
                                Fulfilled
                            </Badge>
                            </TableCell>
                            <TableCell className="hidden md:table-cell">
                            2023-06-25
                            </TableCell>
                            <TableCell className="text-right">$350.00</TableCell>
                        </TableRow>
                        <TableRow>
                            <TableCell>
                            <div className="font-medium">Emma Brown</div>
                            <div className="hidden text-sm text-muted-foreground md:inline">
                                emma@example.com
                            </div>
                            </TableCell>
                            <TableCell className="hidden sm:table-cell">
                            Sale
                            </TableCell>
                            <TableCell className="hidden sm:table-cell">
                            <Badge className="text-xs" variant="secondary">
                                Fulfilled
                            </Badge>
                            </TableCell>
                            <TableCell className="hidden md:table-cell">
                            2023-06-26
                            </TableCell>
                            <TableCell className="text-right">$450.00</TableCell>
                        </TableRow>
                        <TableRow>
                            <TableCell>
                            <div className="font-medium">Liam Johnson</div>
                            <div className="hidden text-sm text-muted-foreground md:inline">
                                liam@example.com
                            </div>
                            </TableCell>
                            <TableCell className="hidden sm:table-cell">
                            Sale
                            </TableCell>
                            <TableCell className="hidden sm:table-cell">
                            <Badge className="text-xs" variant="secondary">
                                Fulfilled
                            </Badge>
                            </TableCell>
                            <TableCell className="hidden md:table-cell">
                            2023-06-23
                            </TableCell>
                            <TableCell className="text-right">$250.00</TableCell>
                        </TableRow>
                        <TableRow>
                            <TableCell>
                            <div className="font-medium">Liam Johnson</div>
                            <div className="hidden text-sm text-muted-foreground md:inline">
                                liam@example.com
                            </div>
                            </TableCell>
                            <TableCell className="hidden sm:table-cell">
                            Sale
                            </TableCell>
                            <TableCell className="hidden sm:table-cell">
                            <Badge className="text-xs" variant="secondary">
                                Fulfilled
                            </Badge>
                            </TableCell>
                            <TableCell className="hidden md:table-cell">
                            2023-06-23
                            </TableCell>
                            <TableCell className="text-right">$250.00</TableCell>
                        </TableRow>
                        <TableRow>
                            <TableCell>
                            <div className="font-medium">Olivia Smith</div>
                            <div className="hidden text-sm text-muted-foreground md:inline">
                                olivia@example.com
                            </div>
                            </TableCell>
                            <TableCell className="hidden sm:table-cell">
                            Refund
                            </TableCell>
                            <TableCell className="hidden sm:table-cell">
                            <Badge className="text-xs" variant="outline">
                                Declined
                            </Badge>
                            </TableCell>
                            <TableCell className="hidden md:table-cell">
                            2023-06-24
                            </TableCell>
                            <TableCell className="text-right">$150.00</TableCell>
                        </TableRow>
                        <TableRow>
                            <TableCell>
                            <div className="font-medium">Emma Brown</div>
                            <div className="hidden text-sm text-muted-foreground md:inline">
                                emma@example.com
                            </div>
                            </TableCell>
                            <TableCell className="hidden sm:table-cell">
                            Sale
                            </TableCell>
                            <TableCell className="hidden sm:table-cell">
                            <Badge className="text-xs" variant="secondary">
                                Fulfilled
                            </Badge>
                            </TableCell>
                            <TableCell className="hidden md:table-cell">
                            2023-06-26
                            </TableCell>
                            <TableCell className="text-right">$450.00</TableCell>
                        </TableRow>
                        </TableBody>
                    </Table>                    
                </div>
            )}            
        </>
    )
}