import { Navigate, Outlet } from "react-router-dom";

export const ProtectedRoute = ({
    // eslint-disable-next-line react/prop-types
    isAllowed,
    // eslint-disable-next-line react/prop-types
    children,
    // eslint-disable-next-line react/prop-types
    redirectTo = "/",
}) => {
    if (!isAllowed) {
        return <Navigate to={redirectTo} />;
    }

    return children ? children : <Outlet />;
};
