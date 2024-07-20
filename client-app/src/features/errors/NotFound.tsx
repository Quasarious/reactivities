import React from "react";
import { Link } from "react-router-dom";
import { Button, Header, Icon, Segment } from "semantic-ui-react";

export default function NotFound() {
    return (
        <Segment placeholder>
            <Header icon>
                <Icon name='search'/>
                Упс... - ничего не найдено, попробуйте снова!
            </Header>
            <Segment.Inline>
                <Button as={Link} to='/activities' primary>
                    Вернуться к списку событий
                </Button>
            </Segment.Inline>
        </Segment>
    )
}