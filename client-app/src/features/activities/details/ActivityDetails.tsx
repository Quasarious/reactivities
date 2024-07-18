import React, { useEffect } from "react";
import { Image, Card, CardContent, CardHeader, CardMeta, CardDescription, Button } from "semantic-ui-react";
import { useStore } from "../../../app/stores/store";
import LoadingComponent from "../../../app/layout/LoadingComponent";
import { useParams } from "react-router-dom";
import { observer } from "mobx-react-lite";
import { Link } from "react-router-dom";


export default observer ( function ActivityDetails() {

    const {activityStore} = useStore();
    const {selectedActivity: activity, loadActivity, loadingInitial} = activityStore;
    const {id} = useParams<{id: string}>();

    useEffect(() => {
        if (id) loadActivity(id);
    }, [id, loadActivity]);

    if (loadingInitial || !activity) return <LoadingComponent content={""} />;

    return (
        <Card fluid>
            <Image src={`/assets/categoryImages/${activity.category}.jpg`} wrapped ui={false} />
            <CardContent>
            <CardHeader>{activity.title}</CardHeader>
            <CardMeta>
                <span className='date'>{activity.date}</span>
            </CardMeta>
            <CardDescription>
                {activity.description}
            </CardDescription>
            </CardContent>
            <CardContent extra>
                <Button.Group widths='2'>
                    <Button as={Link} to={`/manage/${activity.id}`} basic color='blue' content='Edit'/>
                    <Button as= {Link} to='/activities'  basic color='grey' content='Cancel'/>
                </Button.Group>
            </CardContent>
        </Card>
    )
})