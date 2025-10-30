import { useNavigate } from "react-router-dom";
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";
import { Badge } from "@/components/ui/badge";

interface Visit {
  id: string;
  visit_date: string;
  customer: {
    id: string;
    name: string;
    is_loyal: boolean;
  } | null;
  hotel: {
    id: string;
    name: string;
  } | null;
}

interface VisitsDataGridProps {
  visits: Visit[];
  isLoading: boolean;
}

export const VisitsDataGrid = ({ visits, isLoading }: VisitsDataGridProps) => {
  const navigate = useNavigate();

  if (isLoading) {
    return (
      <div className="flex items-center justify-center py-12">
        <div className="text-muted-foreground">Loading visits...</div>
      </div>
    );
  }

  if (visits.length === 0) {
    return (
      <div className="flex items-center justify-center py-12">
        <div className="text-center space-y-2">
          <p className="text-muted-foreground">No visits found</p>
          <p className="text-sm text-muted-foreground">Try adjusting your search filters</p>
        </div>
      </div>
    );
  }

  return (
    <div className="space-y-4">
      <h2 className="text-xl font-semibold">Visit Records</h2>
      <div className="rounded-md border">
        <Table>
          <TableHeader>
            <TableRow>
              <TableHead className="w-20">#</TableHead>
              <TableHead>Customer Name</TableHead>
              <TableHead>Visit Date</TableHead>
              <TableHead>Hotel</TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>
            {visits.map((visit, index) => (
              <TableRow key={visit.id}>
                <TableCell className="font-medium">{index + 1}</TableCell>
                <TableCell>
                  <button
                    onClick={() => navigate(`/profile/${visit.customer?.id}`)}
                    className="text-primary hover:underline font-medium flex items-center gap-2"
                  >
                    {visit.customer?.name}
                    {visit.customer?.is_loyal && (
                      <Badge variant="secondary" className="text-xs">
                        Loyal
                      </Badge>
                    )}
                  </button>
                </TableCell>
                <TableCell>
                  {new Date(visit.visit_date).toLocaleDateString()}
                </TableCell>
                <TableCell>{visit.hotel?.name}</TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </div>
    </div>
  );
};